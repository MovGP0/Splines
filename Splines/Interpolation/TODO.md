# Interpolation Methods

> This is a TODO list and for further interpolation methods 

## Control-point interpolation (same spline basis)

**Idea:** Make both curves share the *same degree, knot vector,* and number of control points; then linearly interpolate control points (and weights for NURBS).

* **Formula (NURBS/B-spline):** After degree elevation + knot insertion to get identical bases,
  $$
  \mathbf{P}_i(t)=(1-t),\mathbf{P}^0_i+t,\mathbf{P}^1_i,\quad
  w_i(t)=(1-t),w^0_i+t,w^1_i.
  $$
  Evaluate the curve with $\mathbf{P}_i(t),w_i(t)$.
* **Pros:** Simple, fast, exact spline at every $t$, preserves continuity class.
* **Cons:** Requires matching bases; visual correspondence depends on how control points correspond.
* **Use when:** Both shapes already modeled as compatible NURBS/B-splines or you can safely reparameterize them to be.

### Example
```csharp
using tinyspline;

public static class NurbsMorph
{
    // Make bases compatible (degree + knots), then lerp control points (and weights).
    public static BSpline Morph(BSpline c0, BSpline c1, double t)
    {
        // Elevate to same degree (TinySpline: elevateDegree)
        while (c0.Degree() < c1.Degree()) c0 = c0.elevateDegree(1);
        while (c1.Degree() < c0.Degree()) c1 = c1.elevateDegree(1);

        // Refine knot vectors until identical (insert missing knots)
        var u0 = c0.Knots(); var u1 = c1.Knots();
        foreach (var k in u1) c0 = c0.insertKnot(k, 1);
        foreach (var k in u0) c1 = c1.insertKnot(k, 1);

        // Now both have same #control points and basis
        int n = (int)c0.NbControlPoints();
        var result = new BSpline(c0); // copy basis

        for (int i = 0; i < n; i++)
        {
            var p0 = c0.ControlPointVec4At(i); // (x,y,z,w) supports rational
            var p1 = c1.ControlPointVec4At(i);
            var p  = new Vec4(
                (1 - t) * p0.x + t * p1.x,
                (1 - t) * p0.y + t * p1.y,
                (1 - t) * p0.z + t * p1.z,
                (1 - t) * p0.w + t * p1.w   // lerp weights for NURBS
            );
            result.setControlPointVec4At(i, p);
        }
        return result;
    }
}
```

### Sources
- [Efficient Degree Elevation and Knot Insertion for B-spline Curves using Derivatives](https://cg.cs.tsinghua.edu.cn/~shimin/pdf/cad04_bspline.pdf)
- [TinySpline: BSpline](https://msteinbeck.github.io/tinyspline/classtinyspline_1_1BSpline.html)

## Pointwise interpolation with parameter alignment

**Idea:** Sample both curves with a shared parameter $u\in[0,1]$, but align them first so that corresponding samples represent the “same” place on the shapes.

* **Alignment choices $\phi_0,\phi_1$:**

    * **Arc-length normalization** (uniform by length).
    * **Dynamic Time Warping (DTW)** / monotone reparameterization minimizing distance.
    * **Feature/landmark alignment** (corners, curvature extrema, endpoints).
* **Morph:**
  $$
  \mathbf{C}(u,t)=(1-t),\mathbf{C}_0(\phi_0(u)) + t,\mathbf{C}_1(\phi_1(u)).
  $$
  Optionally refit a spline to the interpolated samples at each $t$.
* **Pros:** Works for any curve types; good control of correspondences.
* **Cons:** Needs a robust alignment; can cause area/length drift, self-intersections.
* **Use when:** Curves are different types or you want fine control via landmarks.

### Example
```csharp
// Assumes ISpline has Evaluate(u) -> Vector (float/double) and you can sample arc-length.
// If you don’t, uniform parameter sampling + cumulative length works too.
public static class SampleAlignMorph
{
    public static Vector3[] MorphSamples(ISpline c0, ISpline c1, int samples, double t)
    {
        var a0 = ArcLengthSampler.Sample(c0, samples); // P0[i]
        var a1 = ArcLengthSampler.Sample(c1, samples); // P1[i]

        // Build cost matrix for DTW on Euclidean distances
        // Use e.g., FastDtw for speed, or implement classic DTW.
        int n = a0.Length, m = a1.Length;
        var path = DtwWrapper.Align(a0, a1); // returns pairs (i,j)

        // Interpolate along the alignment
        var outPts = new Vector3[path.Count];
        for (int k = 0; k < path.Count; k++)
        {
            (int i, int j) = path[k];
            outPts[k] = Lerp(a0[i], a1[j], t);
        }
        return outPts;
    }

    public static ISpline FitSpline(Vector3[] pts, int degree = 3) =>
        LeastSquaresBspline.Fit(pts, degree); // your repo fitting util
}
```

### LeastSquaresBspline.Fit
- penalized regression spline via ALGLIB

```csharp
using System;
using System.Numerics;
using alglib;

public static class BsplineLeastSquares
{
    /// <summary>
    /// Fit a planar B-spline of given basis size to samples, using penalized LS.
    /// k = spline degree (e.g., 3), m = number of basis functions (>= k+1),
    /// rho = regularization (smoothness) weight (0 = pure LS).
    /// </summary>
    public static (spline1d sX, spline1d sY) Fit(Vector2[] samples, int k = 3, int m = 20, double rho = 1e-3)
    {
        // parameterize by cumulative arc length normalized to [0,1]
        int n = samples.Length;
        var t = new double[n];
        var x = new double[n];
        var y = new double[n];

        t[0] = 0; x[0] = samples[0].X; y[0] = samples[0].Y;
        double L = 0;
        for (int i = 1; i < n; i++)
        {
            L += Vector2.Distance(samples[i], samples[i - 1]);
            x[i] = samples[i].X;
            y[i] = samples[i].Y;
            t[i] = L;
        }
        if (L > 0) for (int i = 0; i < n; i++) t[i] /= L;

        // fit X(t)
        spline1d sx;
        {
            int info;
            spline1dfitreport rep;
            // knots: equidistant in [0,1]
            double[] xc = VectorOnGrid(m);
            double[] yc = new double[m]; // not used (output)
            info = 0;
            sx = null;

            alglib.spline1dfitpenalized(t, x, m, k, rho, out info, out sx, out rep);
            if (info <= 0) throw new InvalidOperationException("ALGLIB spline1dfitpenalized failed for X");
        }

        // fit Y(t)
        spline1d sy;
        {
            int info;
            spline1dfitreport rep;
            alglib.spline1dfitpenalized(t, y, m, k, rho, out info, out sy, out rep);
            if (info <= 0) throw new InvalidOperationException("ALGLIB spline1dfitpenalized failed for Y");
        }

        return (sx, sy);
    }

    public static Vector2[] Evaluate((spline1d sX, spline1d sY) s, int count)
    {
        var pts = new Vector2[count];
        for (int i = 0; i < count; i++)
        {
            double u = (double)i / (count - 1);
            pts[i] = new Vector2((float)alglib.spline1dcalc(s.sX, u),
                                 (float)alglib.spline1dcalc(s.sY, u));
        }
        return pts;
    }

    static double[] VectorOnGrid(int m)
    {
        var v = new double[m];
        for (int i = 0; i < m; i++) v[i] = (double)i / (m - 1);
        return v;
    }
}
```

### Sources
- [Dynamic Time Warping (DTW)](https://github.com/doblak/ndtw)
- [Numerics.NET: Cubic Splines in C# QuickStart Sample](https://numerics.net/quickstart/csharp/cubic-splines)
- [Alglib: Spline interpolation and fitting](https://www.alglib.net/interpolation/spline3.php)

## Decompose: Global transform + residual (Procrustes + displacement)

**Idea:** First interpolate an affine (or rigid/similarity) transform from $\mathbf{C}_0$ to $\mathbf{C}_1$, then interpolate the residual differences.

* **Steps:** Estimate $A,b$ with Procrustes to align $C_0\to C_1$. Morph $A_t,b_t$ linearly, apply to $C_0$, then add residual displacement that blends in.
* **Pros:** Removes bulk motion (scale/rotation/translation) cleanly; smoother morphs.
* **Cons:** Still needs correspondence for residuals.
* **Use when:** Shapes differ by strong rigid/similarity motion + moderate deformation.

### Example
```csharp
using Accord.Statistics.Analysis;
using Accord.Math;

public static class ProcrustesPreAlign
{
    public static (double[,] XAligned, double[,] Y) Align(double[,] X, double[,] Y)
    {
        // X,Y are NxD point clouds (e.g., samples along c0, c1)
        var pa = new ProcrustesAnalysis(X, Y, ProcrustesAnalysisMethod.Similarity);
        pa.Compute();
        var Xhat = pa.Transforms[0].Transform(X); // X mapped onto Y
        return (Xhat, Y);
    }

    public static Vector3[] Morph(Vector3[] xAligned, Vector3[] y, double t)
    {
        var pts = new Vector3[xAligned.Length];
        for (int i = 0; i < pts.Length; i++)
            pts[i] = Lerp(xAligned[i], y[i], t);
        return pts;
    }
}
```

### Sources
- [Accord.NET: ProcrustesAnalysis Class](https://accord-framework.net/docs/html/T_Accord_Statistics_Analysis_ProcrustesAnalysis.htm)

## **Free-Form Deformation (FFD)** / lattice morph

**Idea:** Embed curve in a B-spline lattice; interpolate the lattice control points.

* **Morph:** Define a single lattice $L$; find two lattice configurations $L^0,L^1$ that place each curve; then
  $$
  L(t)=(1-t)L^0+tL^1,\quad \mathbf{x}(t)=\text{FFD}(\mathbf{x};L(t)).
  $$
* **Pros:** Smooth, low-dimensional control; preserves topology; easy constraints.
* **Cons:** Needs solving for lattices that match each shape; may require optimization.
* **Use when:** You want smooth global deformations with a few handles (CAD/DCC workflows).

### Example
```csharp
// Minimal FFD lattice morph around a curve: evaluate world->lattice, blend lattice CPs, apply FFD.
// Usage: build two lattices L0,L1 that “fit” C0 and C1, then L(t)=Lerp(L0,L1,t), deform samples, refit spline.
public sealed class FfdLattice
{
    // 3D lattice of control points C[u,v,w] with B-spline basis of degree p
    private readonly Vector3[,,] controlPoints;
    private readonly int du, dv, dw; // counts
    private readonly int p = 3;

    public FfdLattice(Vector3[,,] cps) { controlPoints = cps; (du,dv,dw) = (cps.GetLength(0), cps.GetLength(1), cps.GetLength(2)); }

    // Evaluate FFD deformation at local lattice coords (s,t,r) in [0,1]^3
    public Vector3 Deform(Vector3 xLocal)
    {
        var (s,t,r) = (xLocal.X, xLocal.Y, xLocal.Z);
        Vector3 sum = default;
        for (int i=0;i<du;i++)
        for (int j=0;j<dv;j++)
        for (int k=0;k<dw;k++)
        {
            double Bi = BSplineBasis.Evaluate(p, i, s, du);
            double Bj = BSplineBasis.Evaluate(p, j, t, dv);
            double Bk = BSplineBasis.Evaluate(p, k, r, dw);
            sum += (float)(Bi*Bj*Bk) * controlPoints[i,j,k];
        }
        return sum;
    }

    public static FfdLattice Lerp(FfdLattice a, FfdLattice b, double t)
    {
        var cps = (Vector3[,,])a.controlPoints.Clone();
        for (int i=0;i<cps.GetLength(0);i++)
        for (int j=0;j<cps.GetLength(1);j++)
        for (int k=0;k<cps.GetLength(2);k++)
            cps[i,j,k] = Lerp(a.controlPoints[i,j,k], b.controlPoints[i,j,k], t);
        return new FfdLattice(cps);
    }
}
```

### Sources
- [MIT: Free-Form Deformation of Parametric CAD Geometry via B-Spline Transformations](https://acdl.mit.edu/ESP/Publications/AIAApaper2023-3601.pdf?utm_source=chatgpt.com)

## Thin-Plate Spline (TPS) / RBF warp

**Idea:** Compute a smooth warp field mapping landmarks on curve 0 to landmarks on curve 1; then interpolate the warp.

* **Morph field:** Compute TPS $T$ s.t. $T(\mathbf{p}^0_j)=\mathbf{p}^1_j$. Use
  $$
  T_t(\mathbf{x})=(1-t),\mathbf{x}+t,T(\mathbf{x}),
  $$
  apply to samples of $C_0$, optionally refit a spline per $t$.
* **Pros:** Very smooth, handles large, nonuniform deformations.
* **Cons:** Needs well-distributed correspondences; global influence can cause overshoot.
* **Use when:** You can select meaningful landmarks; want visually smooth warps.

### Example
```csharp
// Minimal TPS using a wrapper class (pseudo-API: ThinPlateSpline2D)
// p_j <-> q_j are 2D landmarks on c0 and c1 (extend to 3D with suitable kernel)
public static class TpsMorph
{
    public static Vector2[] Morph(Vector2[] curve0, (Vector2 P, Vector2 Q)[] landmarks, double t)
    {
        var tps = new ThinPlateSpline2D();
        foreach (var (P,Q) in landmarks) tps.AddPair(P, Q);
        tps.Solve(); // compute weights

        var outPts = new Vector2[curve0.Length];
        for (int i = 0; i < curve0.Length; i++)
        {
            var mapped = tps.Map(curve0[i]);           // T(x)
            outPts[i] = (1 - t) * curve0[i] + t * mapped;
        }
        return outPts;
    }
}
```

### Sources
- [Alglib: Thin plate spline interpolation and fitting](https://www.alglib.net/thin-plate-spline-interpolation-and-fitting/)
- [StackOverflow: Results from my thin plate spline interpolation implementation are dependant of the independent variables](https://stackoverflow.com/questions/42426397/results-from-my-thin-plate-spline-interpolation-implementation-are-dependant-of)

## As-Rigid-As-Possible (ARAP) / Laplacian curve editing

**Idea:** Treat the curve’s polyline with Laplacian coordinates; optimize intermediate shapes to match target constraints while staying locally rigid.

* **Morph:** Minimize$|\Delta \mathbf{C}(t)-\Delta \mathbf{C}_0|$ with soft constraints pulling towards $\mathbf{C}_1$ that increase with $t$.
* **Pros:** Preserves local shape; avoids rubbery artifacts.
* **Cons:** Requires an optimization per $t$; set of constraints must be chosen.
* **Use when:** You care about local rigidity (engineering/character rig-like behavior).
* **Formulation:** Laplacian coordinates + per-edge local rotations; classical references (Sorkine & Alexa; Igarashi et al. include a curve editing note).

### Example
```csharp
// Discrete ARAP for a polyline P with handle constraints H(t) blending from C0->C1.
// 1) Build cotangent/umbrella Laplacian L for the polyline
// 2) Iterate: (a) estimate per-edge rotations R_e; (b) solve (L^T L) X = L^T S with constraints (sparse LSQ)
public static class ArapCurveMorph
{
    public static Vector2[] Morph(Vector2[] P0, Vector2[] P1, double t, int iters = 5)
    {
        var P = (Vector2[])P0.Clone();
        var H = BuildHandleTargets(P0, P1, t); // select landmark indices; linearly interpolate targets

        var L = Laplacian.Build1D(P.Length);   // sparse (N x N)
        for (int it = 0; it < iters; it++)
        {
            var rotations = EstimateEdgeRotations(P, P0); // Sorkine’s local step
            var rhs = Laplacian.AssembleRhs(P0, rotations);

            // Constrained least-squares: (L^T L) X = L^T rhs, with pin constraints for handles H
            P = SolveWithConstraints(L, rhs, H);
        }
        return P;
    }
}
```

**Note:** Use any sparse solver—Math.NET Numerics or your own Cholesky—with hard constraints injected via row replacement.

### Sources
- [As-Rigid-As-Possible Surface Modeling](https://igl.ethz.ch/projects/ARAP/arap_web.pdf)

## Diffeomorphic registration (B-spline velocity field / LDDMM)

**Idea:** Compute a smooth, invertible flow $\Phi_t$ that transports curve 0 onto curve 1; then sample along $t$.

* **Pros:** Topology-preserving, robust to large deformations, strong theory.
* **Cons:** Heavier optimization; more parameters.
* **Use when:** You need guaranteed smooth, invertible morphs (medical/vision).

### Example
```csharp
// Pseudo-C# with SimpleITK: fit BSplineTransform between two distance-field images of curves, then sample Φ_t.
using itk.simple;

public static class DiffeoBsplineMorph
{
    public static (BSplineTransform tx) RegisterBspline(Image fixedImg, Image movingImg, uint[] meshSize)
    {
        var R = new ImageRegistrationMethod();
        var initTx = new BSplineTransform(fixedImg, meshSize, 3); // cubic
        R.SetInitialTransform(initTx, inPlace:false);
        R.SetMetricAsMattesMutualInformation(50);
        R.SetInterpolator(InterpolatorEnum.sitkLinear);
        R.SetOptimizerAsLBFGSB(gradientConvergenceTolerance:1e-5, numberOfIterations:200);
        var outTx = R.Execute(fixedImg, movingImg);
        return ((BSplineTransform)outTx);
    }

    public static Vector2[] ApplyFlow(Vector2[] pts, Transform tx, double t)
    {
        // Blend identity with the diffeomorphic warp (stationary velocity assumption)
        var outPts = new Vector2[pts.Length];
        for (int i = 0; i < pts.Length; i++)
        {
            var y = tx.TransformPoint(pts[i].X, pts[i].Y);
            outPts[i] = (1 - t) * pts[i] + t * new Vector2((float)y[0], (float)y[1]);
        }
        return outPts;
    }
}
```

## Shape-space interpolation (SRVF/elastic metrics)

**Idea:** Map curves to a “shape space” (e.g., Square-Root Velocity Framework), compute geodesics there, then map back.

* **Pros:** Reparameterization-invariant; handles closed/open curves; theoretically grounded.
* **Cons:** Math-heavy; implementation complexity.
* **Use when:** You want correspondence-insensitive, principled shape morphs.

### Example
```csharp
public static class SrbfMorph
{
    // Discrete SRV for open 2D curves
    public static Vector2[] Morph(Vector2[] c0, Vector2[] c1, double t)
    {
        var q0 = ToSrv(c0);
        var q1 = ToSrv(c1);

        // Optionally align by reparameterization (approx via DTW index mapping)
        var (Q0, Q1) = AlignSrvByDtw(q0, q1);

        var qt = Q0.Zip(Q1, (a,b) => (1 - t)*a + t*b).ToArray();
        return FromSrv(qt, c0[0]); // integrate starting from initial point
    }

    static Vector2[] ToSrv(Vector2[] c)
    {
        var q = new Vector2[c.Length - 1];
        for (int i = 0; i < q.Length; i++)
        {
            var v = c[i+1] - c[i];
            var n = Math.Sqrt(v.X*v.X + v.Y*v.Y) + 1e-12;
            q[i] = (float)(1.0 / Math.Sqrt(n)) * v; // q = v / sqrt(|v|)
        }
        return q;
    }

    static Vector2[] FromSrv(Vector2[] q, Vector2 x0)
    {
        var c = new List<Vector2> { x0 };
        foreach (var qi in q)
        {
            var v = qi; var s = v.Length();         // |q| = sqrt(|v|)
            var step = s > 0 ? (qi / (float)(s*s))  : Vector2.Zero; // invert q -> v
            c.Add(c[^1] + step);
        }
        return c.ToArray();
    }
}
```

### Sources
- [Square-Root Velocity (SRV) Representations](https://www.emergentmind.com/topics/square-root-velocity-srv-representations)

## Curvature-driven / feature-aware blending

**Idea:** Align by curvature extrema/zero-crossings, interpolate curvature $\kappa(u)$ and torsion $(\tau(u)$ (3D) plus Frenet frames, then reconstruct.

* **Pros:** Preserves salient features; less control-polygon bias.
* **Cons:** Sensitive to noise; reconstruction requires care.
* **Use when:** Feature fidelity (corners/waves) matters more than raw point proximity.

### Example
```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

public static class CurvatureBlend2D
{
    public static Vector2[] Morph(Vector2[] c0, Vector2[] c1, int sampleCount, double t)
    {
        // 1) Arc-length resample both curves to N samples
        var s0 = ResampleByArclength(c0, sampleCount);
        var s1 = ResampleByArclength(c1, sampleCount);

        // 2) Estimate curvature arrays k0(s), k1(s)
        var k0 = EstimateCurvature2D(s0);
        var k1 = EstimateCurvature2D(s1);

        // 3) Interpolate curvature
        var kt = new double[k0.Length];
        for (int i = 0; i < kt.Length; i++)
            kt[i] = (1 - t) * k0[i] + t * k1[i];

        // 4) Integrate Frenet: θ'(s)=κ(s), x'(s)=(cosθ, sinθ)
        //    Use the first tangent from c0 as initial heading (robust in practice).
        var tangents0 = DiscreteTangents(s0);
        double theta0 = Math.Atan2(tangents0[0].Y, tangents0[0].X);
        var s = UniformArclengths(s0); // Δs steps

        var curve = new Vector2[s.Length];
        curve[0] = (1 - (float)t) * s0[0] + (float)t * s1[0]; // blended start position
        double theta = theta0;

        for (int i = 1; i < s.Length; i++)
        {
            double ds = s[i] - s[i - 1];
            theta += kt[i - 1] * ds; // θ_i ≈ θ_{i-1} + κ*Δs
            var dir = new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta));
            curve[i] = curve[i - 1] + (float)ds * dir;
        }

        // 5) Optional rigid alignment to stabilize drift (recommend)
        //    Align curve to blended endpoints via 2-point Procrustes.
        curve = RigidAlignByEndpoints(curve, s0, s1, t);
        return curve;
    }

    // --- helpers ---

    static Vector2[] ResampleByArclength(IReadOnlyList<Vector2> pts, int n)
    {
        var cum = new double[pts.Count];
        cum[0] = 0;
        for (int i = 1; i < pts.Count; i++)
            cum[i] = cum[i - 1] + Vector2.Distance(pts[i - 1], pts[i]);

        double L = cum[^1];
        var outPts = new Vector2[n];
        for (int i = 0; i < n; i++)
        {
            double s = L * i / (n - 1);
            outPts[i] = SampleAtArclength(pts, cum, s);
        }
        return outPts;
    }

    static Vector2 SampleAtArclength(IReadOnlyList<Vector2> pts, double[] cum, double s)
    {
        int j = Array.BinarySearch(cum, s);
        if (j >= 0) return pts[j];
        j = ~j;
        if (j <= 0) return pts[0];
        if (j >= pts.Count) return pts[^1];
        double t = (s - cum[j - 1]) / (cum[j] - cum[j - 1]);
        return Vector2.Lerp(pts[j - 1], pts[j], (float)t);
    }

    static double[] UniformArclengths(IReadOnlyList<Vector2> pts)
    {
        var cum = new double[pts.Count];
        cum[0] = 0;
        for (int i = 1; i < pts.Count; i++)
            cum[i] = cum[i - 1] + Vector2.Distance(pts[i - 1], pts[i]);
        return cum;
    }

    static Vector2[] DiscreteTangents(IReadOnlyList<Vector2> pts)
    {
        var t = new Vector2[pts.Count];
        for (int i = 0; i < pts.Count; i++)
        {
            Vector2 d;
            if (i == 0) d = pts[1] - pts[0];
            else if (i == pts.Count - 1) d = pts[^1] - pts[^2];
            else d = 0.5f * (pts[i + 1] - pts[i - 1]);
            float len = d.Length();
            t[i] = len > 1e-12f ? d / len : new Vector2(1, 0);
        }
        return t;
    }

    static double[] EstimateCurvature2D(IReadOnlyList<Vector2> pts)
    {
        // Finite-difference estimate of κ = |x'y'' - y'x''| / (x'^2 + y'^2)^(3/2)
        int n = pts.Count;
        var k = new double[n];
        for (int i = 0; i < n; i++)
        {
            Vector2 p0 = i == 0 ? pts[i] : pts[i - 1];
            Vector2 p1 = pts[i];
            Vector2 p2 = i == n - 1 ? pts[i] : pts[i + 1];

            var d1 = (p2 - p0) * 0.5f;       // ~ first derivative (scaled)
            var d2 = p2 - 2 * p1 + p0;       // ~ second derivative (scaled)

            double xp = d1.X, yp = d1.Y;
            double xpp = d2.X, ypp = d2.Y;

            double denom = Math.Pow(xp * xp + yp * yp, 1.5);
            k[i] = denom > 1e-18 ? Math.Abs(xp * ypp - yp * xpp) / denom : 0.0;
        }
        return k;
    }

    static Vector2[] RigidAlignByEndpoints(Vector2[] curve, Vector2[] s0, Vector2[] s1, double t)
    {
        // Align start & end of reconstructed curve to blended endpoints
        var a0 = s0[0]; var a1 = s0[^1];
        var b0 = s1[0]; var b1 = s1[^1];
        var target0 = Vector2.Lerp(a0, b0, (float)t);
        var target1 = Vector2.Lerp(a1, b1, (float)t);

        var cur0 = curve[0];
        var cur1 = curve[^1];

        // Compute similarity: translation + uniform scale + rotation from (cur0,cur1) -> (target0,target1)
        var u = cur1 - cur0; var v = target1 - target0;
        float nu = u.Length();
        if (nu < 1e-9f) return curve;
        float nv = v.Length();
        float s = nv / nu;
        float cos = 0, sin = 0;
        if (nu > 0 && nv > 0)
        {
            u /= nu; v /= nv;
            cos = Vector2.Dot(u, v);
            sin = u.X * v.Y - u.Y * v.X;
        }

        var outPts = new Vector2[curve.Length];
        for (int i = 0; i < curve.Length; i++)
        {
            var x = curve[i] - cur0;
            var xr = new Vector2(cos * x.X - sin * x.Y, sin * x.X + cos * x.Y);
            outPts[i] = target0 + s * xr;
        }
        return outPts;
    }
}
```

### Sources
- [Frenet–Serret formulas](https://en.wikipedia.org/wiki/Frenet%E2%80%93Serret_formulas)

##  Wavelet/multiresolution morph

**Idea:** Decompose curve into coarse + detail; interpolate coarse geometry, then blend details (possibly with different easing).

* **Pros:** Stable coarse behavior, controllable detail fade-in/out.
* **Cons:** Extra decomposition and bookkeeping.
* **Use when:** You want art-directable morphs (e.g., keep high-freq detail late).

### Example
```csharp
// Install-Package Accord
// Install-Package Accord.Math

using System;
using System.Numerics;
using Accord.Math.Wavelets;

public static class Wavelet
{
    // Simple multi-level Haar decomposition per coordinate.
    // Returns approximation + concatenated detail bands for each coordinate.
    public static (double[] approxX, double[][] detailsX,
                   double[] approxY, double[][] detailsY)
        Decompose(Vector2[] samples, int levels)
    {
        double[] x = new double[samples.Length];
        double[] y = new double[samples.Length];
        for (int i = 0; i < samples.Length; i++) { x[i] = samples[i].X; y[i] = samples[i].Y; }

        var haar = new Haar(); // 1-D Haar wavelet
        var (ax, dx) = Decompose1D(x, levels, haar);
        var (ay, dy) = Decompose1D(y, levels, haar);
        return (ax, dx, ay, dy);
    }

    public static Vector2[] Reconstruct((double[] approxX, double[][] detailsX,
                                         double[] approxY, double[][] detailsY) w)
    {
        var haar = new Haar();
        double[] rx = Reconstruct1D(w.approxX, w.detailsX, haar);
        double[] ry = Reconstruct1D(w.approxY, w.detailsY, haar);

        var outPts = new Vector2[rx.Length];
        for (int i = 0; i < rx.Length; i++) outPts[i] = new Vector2((float)rx[i], (float)ry[i]);
        return outPts;
    }

    // --- internals: 1-D multilevel ---
    private static (double[] approx, double[][] details) Decompose1D(double[] data, int levels, IWavelet wavelet)
    {
        var details = new double[levels][];
        double[] current = (double[])data.Clone();
        for (int l = 0; l < levels; l++)
        {
            // Forward transform in-place on "current"
            double[] approx, detail;
            ForwardHaarOnce(current, wavelet, out approx, out detail);
            details[l] = detail;
            current = approx;
        }
        return (current, details);
    }

    private static double[] Reconstruct1D(double[] approx, double[][] details, IWavelet wavelet)
    {
        double[] current = (double[])approx.Clone();
        for (int l = details.Length - 1; l >= 0; l--)
            current = InverseHaarOnce(current, details[l], wavelet);
        return current;
    }

    // Single-level forward Haar: split into approx/detail
    private static void ForwardHaarOnce(double[] signal, IWavelet wavelet,
                                        out double[] approx, out double[] detail)
    {
        int n = signal.Length;
        if ((n & 1) == 1) throw new ArgumentException("Signal length must be even for 1-level Haar.");
        approx = new double[n / 2];
        detail = new double[n / 2];

        // The Haar object exposes analysis/synthesis filters; here we do the classical average/difference.
        // (For arbitrary IWavelet, apply its filterbank; Haar reduces to (a=(x0+x1)/√2, d=(x0-x1)/√2).)
        double invSqrt2 = 1.0 / Math.Sqrt(2);
        for (int i = 0, j = 0; j < n; i++, j += 2)
        {
            double a = (signal[j] + signal[j + 1]) * invSqrt2;
            double d = (signal[j] - signal[j + 1]) * invSqrt2;
            approx[i] = a; detail[i] = d;
        }
    }

    // Single-level inverse Haar: merge approx/detail back to signal
    private static double[] InverseHaarOnce(double[] approx, double[] detail, IWavelet wavelet)
    {
        int n = approx.Length + detail.Length;
        var signal = new double[n];
        double invSqrt2 = 1.0 / Math.Sqrt(2);
        for (int i = 0, j = 0; i < approx.Length; i++, j += 2)
        {
            signal[j]     = (approx[i] + detail[i]) * invSqrt2;
            signal[j + 1] = (approx[i] - detail[i]) * invSqrt2;
        }
        return signal;
    }
}
```

```csharp
// 1) Decompose two arc-length sampled curves:
var W0 = Wavelet.Decompose(samples0, levels: 3);
var W1 = Wavelet.Decompose(samples1, levels: 3);

// 2) Interpolate coarse + detail with different easing:
double ce = SmoothStep(t);
double de = SmoothStep(Math.Clamp((t - 0.5) * 2, 0, 1)); // details fade in late

double[] ax = Lerp(W0.approxX, W1.approxX, ce);
double[] ay = Lerp(W0.approxY, W1.approxY, ce);
var dx = LerpBands(W0.detailsX, W1.detailsX, de);
var dy = LerpBands(W0.detailsY, W1.detailsY, de);

// 3) Reconstruct samples_t:
var samples_t = Wavelet.Reconstruct((ax, dx, ay, dy));

// 4) Fit a B-spline at each t (for a spline output instead of a polyline):
var fit = BsplineLeastSquares.Fit(samples_t, k:3, m:Math.Max(12, samples_t.Length/4), rho:1e-3);
var splineSamples = BsplineLeastSquares.Evaluate(fit, 200);

// --- helpers ---
static double[] Lerp(double[] a, double[] b, double t)
{
    var r = new double[a.Length];
    for (int i = 0; i < a.Length; i++) r[i] = (1 - t) * a[i] + t * b[i];
    return r;
}
static double[][] LerpBands(double[][] a, double[][] b, double t)
{
    var r = new double[a.Length][];
    for (int l = 0; l < a.Length; l++)
        r[l] = Lerp(a[l], b[l], t);
    return r;
}
static double SmoothStep(double x) => x * x * (3 - 2 * x);
```

### Sources
- [Accord: Haar](https://accord-framework.net/docs/html/T_Accord_Math_Wavelets_Haar.htm)
- [Mathematics of Image and Data Analysis: The Discrete Wavelet Transform](https://www-users.cse.umn.edu/~jwcalder/5467/lec_dwt.pd)

## Practical selection guide

* **Both curves are NURBS/B-spline and similar topology:**
  **(1) Control-point interpolation** after **degree/knot unification**. Add **(3)** for better bulk motion, and **ease** $t$ (e.g., smoothstep).

* **Different curve types / you want robust “just works”:**
  **(2) Parameter-aligned pointwise interpolation** (arc-length or DTW) + optional **refit** at each ($t$. Add **landmarks** to stabilize.

* **Need smooth nonuniform warps from sparse correspondences:**
  **(5) TPS/RBF warp** or **(4) FFD**.

* **Preserve local rigidity / minimize distortion:**
  **(6) ARAP/Laplacian editing**.

* **Guarantee invertibility / medical-grade smoothness:**
  **(7) Diffeomorphic registration**.

* **Correspondence should be “automatically fair”:**
  **(8) Shape-space geodesics (SRVF)**.

---

## Implementation notes & pitfalls

* **Correspondence is king.** Even for (1), matching control points matters. For 2/5, pick landmarks at curvature extrema, endpoints, junctions.
* **Topology & closure:** Treat open vs closed curves separately; for closed, optimize a cyclic shift to align starts.
* **Self-intersections:** Add regularizers (fairness/elastic energy) or monotone normal-offset constraints; clamp step size.
* **Parameterization:** Prefer **arc-length**; DTW helps when local speeds differ.
* **Constraints:** Preserve length/area by adding penalties, or enforce via projection after each step.
* **Easing:** Replace $t$ by $e(t)$ (e.g., $3t^2-2t^3$) for visually smooth in/out.
* **3D frames:** If you blend in local frames, use **quaternion slerp** for rotations.

---

## Minimal algorithms (sketch)

**A. Compatible NURBS morph**

1. Elevate degree and insert knots so both curves share $(p,U)$.
2. Reorder/control-point pairing consistently.
3. For $t\in[0,1]$: interpolate control points and weights; evaluate curve.

**B. Sample-align-refit**

1. Arc-length sample $N$ points on each curve.
2. Find $\phi$ via DTW (or landmark spline).
3. For each $t$: $\mathbf{x}*i(t)=(1-t),\mathbf{x}^0*{\phi(i)}+t,\mathbf{x}^1_i$.
4. Fit a spline to $\mathbf{x}_i(t)$ (least-squares B-spline).

**C. TPS warp**

1. Pick (M) corresponding landmarks ${\mathbf{p}^0_j}\leftrightarrow{\mathbf{p}^1_j}$.
2. Solve TPS (T).
3. For (t): deform samples of (C_0) by $T_t(\mathbf{x})=(1-t)\mathbf{x}+tT(\mathbf{x})$; refit spline.
