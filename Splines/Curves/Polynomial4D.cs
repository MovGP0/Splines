using System.Numerics;
using System.Runtime.CompilerServices;
using Splines.Extensions;
using Splines.Numerics;
using Splines.Splines;
using Splines.Splines.UniformSplineSegments;

namespace Splines.Curves;

public struct Polynomial4D : IPolynomialCubic<Polynomial4D, Vector4>, IParamCurve4Diff<Vector4>
{
    /// <inheritdoc cref="Polynomial1D.NaN"/>
    public static readonly Polynomial4D NaN = new()
    {
        X = Polynomial1D.NaN,
        Y = Polynomial1D.NaN,
        Z = Polynomial1D.NaN,
        W = Polynomial1D.NaN
    };

    public Polynomial1D X, Y, Z, W;

    public Polynomial4D(Polynomial1D x, Polynomial1D y, Polynomial1D z, Polynomial1D w) => (X, Y, Z, W) = (x, y, z, w);

    /// <inheritdoc cref="Polynomial1D(float,float,float,float)"/>
    public Polynomial4D(Vector4 c0, Vector4 c1, Vector4 c2, Vector4 c3) {
        X = new Polynomial1D(c0.X, c1.X, c2.X, c3.X);
        Y = new Polynomial1D(c0.Y, c1.Y, c2.Y, c3.Y);
        Z = new Polynomial1D(c0.Z, c1.Z, c2.Z, c3.Z);
        W = new Polynomial1D(c0.W, c1.W, c2.W, c3.W);
    }

    /// <inheritdoc cref="Polynomial1D(float,float,float)"/>
    public Polynomial4D(Vector4 c0, Vector4 c1, Vector4 c2) {
        X = new Polynomial1D(c0.X, c1.X, c2.X, 0);
        Y = new Polynomial1D(c0.Y, c1.Y, c2.Y, 0);
        Z = new Polynomial1D(c0.Z, c1.Z, c2.Z, 0);
        W = new Polynomial1D(c0.W, c1.W, c2.W, 0);
    }

    /// <inheritdoc cref="Polynomial1D(float,float)"/>
    public Polynomial4D(Vector4 c0, Vector4 c1) {
        X = new Polynomial1D(c0.X, c1.X, 0, 0);
        Y = new Polynomial1D(c0.Y, c1.Y, 0, 0);
        Z = new Polynomial1D(c0.Z, c1.Z, 0, 0);
        W = new Polynomial1D(c0.W, c1.W, 0, 0);
    }

    /// <inheritdoc cref="Polynomial1D(Matrix4x1)"/>
    public Polynomial4D(Vector4Matrix4x1 coefficients) => (X, Y, Z, W) = (new Polynomial1D(coefficients.X), new Polynomial1D(coefficients.Y), new Polynomial1D(coefficients.Z), new Polynomial1D(coefficients.W));

    /// <inheritdoc cref="Polynomial1D(Matrix4x1)"/>
    public Polynomial4D(Vector4Matrix3x1 coefficients) => (X, Y, Z, W) = (new Polynomial1D(coefficients.X), new Polynomial1D(coefficients.Y), new Polynomial1D(coefficients.Z), new Polynomial1D(coefficients.W));

    #region IPolynomialCubic

    public Vector4 C0
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(X.c0, Y.c0, Z.c0, W.c0);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => (X.c0, Y.c0, Z.c0, W.c0) = (value.X, value.Y, value.Z, value.W);
    }

    public Vector4 C1
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(X.c1, Y.c1, Z.c1, W.c1);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => (X.c1, Y.c1, Z.c1, W.c1) = (value.X, value.Y, value.Z, value.W);
    }

    public Vector4 C2
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(X.c2, Y.c2, Z.c2, W.c2);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => (X.c2, Y.c2, Z.c2, W.c2) = (value.X, value.Y, value.Z, value.W);
    }

    public Vector4 C3
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(X.c3, Y.c3, Z.c3, W.c3);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => (X.c3, Y.c3, Z.c3, W.c3) = (value.X, value.Y, value.Z, value.W);
    }

    public Polynomial1D this[int i]
    {
        get => i switch
        {
            0 => X,
            1 => Y,
            2 => Z,
            4 => W,
            _ => throw new IndexOutOfRangeException("Polynomial4D component index has to be either 0, 1, 2, or 3")
        };
        set => _ = i switch
        {
            0 => X = value,
            1 => Y = value,
            2 => Z = value,
            3 => W = value,
            _ => throw new IndexOutOfRangeException()
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector4 GetCoefficient(int degree) =>
        degree switch
        {
            0 => C0,
            1 => C1,
            2 => C2,
            3 => C3,
            _ => throw new IndexOutOfRangeException("Polynomial coefficient degree/index has to be between 0 and 3")
        };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetCoefficient(int degree, Vector4 value)
    {
        _ = degree switch
        {
            0 => C0 = value,
            1 => C1 = value,
            2 => C2 = value,
            3 => C3 = value,
            _ => throw new IndexOutOfRangeException("Polynomial coefficient degree/index has to be between 0 and 3")
        };
    }

    public Vector4 Eval(float t) {
        float t2 = t * t;
        float t3 = t2 * t;
        return new Vector4(
            X.c3 * t3 + X.c2 * t2 + X.c1 * t + X.c0,
            Y.c3 * t3 + Y.c2 * t2 + Y.c1 * t + Y.c0,
            Z.c3 * t3 + Z.c2 * t2 + Z.c1 * t + Z.c0,
            W.c3 * t3 + W.c2 * t2 + W.c1 * t + W.c0
       );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector4 Eval(float t, int n) => Differentiate(n).Eval(t);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Polynomial4D Differentiate(int n = 1) => new(X.Differentiate(n), Y.Differentiate(n), Z.Differentiate(n), W.Differentiate(n));

    public Polynomial4D ScaleParameterSpace(float factor)
    {
        // ReSharper disable once CompareOfFloatsByEqualityOperator
        if (factor == 1f)
            return this;
        float factor2 = factor * factor;
        float factor3 = factor2 * factor;
        return new Polynomial4D(
            new Polynomial1D(X.c0, X.c1 / factor, X.c2 / factor2, X.c3 / factor3),
            new Polynomial1D(Y.c0, Y.c1 / factor, Y.c2 / factor2, Y.c3 / factor3),
            new Polynomial1D(Z.c0, Z.c1 / factor, Z.c2 / factor2, Z.c3 / factor3),
            new Polynomial1D(W.c0, W.c1 / factor, W.c2 / factor2, W.c3 / factor3)
       );
    }

    public Polynomial4D Compose(float g0, float g1) => new(X.Compose(g0, g1), Y.Compose(g0, g1), Z.Compose(g0, g1), W.Compose(g0, g1));

    #endregion

    public static Polynomial4D FitCubicFrom0(
        float x1,
        float x2,
        float x3,
        Vector4 y0,
        Vector4 y1,
        Vector4 y2,
        Vector4 y3)
    {
        // precalcs
        float i12 = x2 - x1;
        float i13 = x3 - x1;
        float i23 = x3 - x2;
        float x1x2 = x1 * x2;
        float x1x3 = x1 * x3;
        float x2x3 = x2 * x3;
        float x1x2x3 = x1 * x2x3;
        float x0plusx1plusx2 = x1 + x2;
        float x0plusx1plusx3 = x1 + x3;
        float x2plusx3 = x2 + x3;
        float x1plusx2plusx3 = x1 + x2plusx3;
        float x1x2plusx1x3plusx2x3 = (x1x2 + x1x3 + x2x3);

        // scale factors
        Vector4 scl0 = y0 / -(x1 * x2 * x3);
        Vector4 scl1 = y1 / +(x1 * i12 * i13);
        Vector4 scl2 = y2 / -(x2 * i12 * i23);
        Vector4 scl3 = y3 / +(x3 * i13 * i23);

        // polynomial form
        Vector4 c0 = new(
            -(scl0.X * x1x2x3),
            -(scl0.Y * x1x2x3),
            -(scl0.Z * x1x2x3),
            -(scl0.W * x1x2x3)
       );
        Vector4 c1 = new(
            scl0.X * x1x2plusx1x3plusx2x3 + scl1.X * x2x3 + scl2.X * x1x3 + scl3.X * x1x2,
            scl0.Y * x1x2plusx1x3plusx2x3 + scl1.Y * x2x3 + scl2.Y * x1x3 + scl3.Y * x1x2,
            scl0.Z * x1x2plusx1x3plusx2x3 + scl1.Z * x2x3 + scl2.Z * x1x3 + scl3.Z * x1x2,
            scl0.W * x1x2plusx1x3plusx2x3 + scl1.W * x2x3 + scl2.W * x1x3 + scl3.W * x1x2
       );
        Vector4 c2 = new(
            -(scl0.X * x1plusx2plusx3 + scl1.X * x2plusx3 + scl2.X * x0plusx1plusx3 + scl3.X * x0plusx1plusx2),
            -(scl0.Y * x1plusx2plusx3 + scl1.Y * x2plusx3 + scl2.Y * x0plusx1plusx3 + scl3.Y * x0plusx1plusx2),
            -(scl0.Z * x1plusx2plusx3 + scl1.Z * x2plusx3 + scl2.Z * x0plusx1plusx3 + scl3.Z * x0plusx1plusx2),
            -(scl0.W * x1plusx2plusx3 + scl1.W * x2plusx3 + scl2.W * x0plusx1plusx3 + scl3.W * x0plusx1plusx2)
       );
        Vector4 c3 = new(
            scl0.X + scl1.X + scl2.X + scl3.X,
            scl0.Y + scl1.Y + scl2.Y + scl3.Y,
            scl0.Z + scl1.Z + scl2.Z + scl3.Z,
            scl0.W + scl1.W + scl2.W + scl3.W
       );

        return new Polynomial4D(c0, c1, c2, c3);
    }

    /// <inheritdoc cref="Polynomial2D.GetBounds01"/>
    public (FloatRange x, FloatRange y, FloatRange z, FloatRange w) GetBounds01()
        => (X.OutputRange01, Y.OutputRange01, Z.OutputRange01, W.OutputRange01);

    /// <inheritdoc cref="Polynomial1D.Split01"/>
    public (Polynomial4D pre, Polynomial4D post) Split01(float u)
    {
        (Polynomial1D xPre, Polynomial1D xPost) = X.Split01(u);
        (Polynomial1D yPre, Polynomial1D yPost) = Y.Split01(u);
        (Polynomial1D zPre, Polynomial1D zPost) = Z.Split01(u);
        (Polynomial1D wPre, Polynomial1D wPost) = W.Split01(u);

        return (
            new Polynomial4D(xPre, yPre, zPre, wPre),
            new Polynomial4D(xPost, yPost, zPost, wPost));
    }

    #region IParamCurve3Diff interface implementations

    public int Degree => Mathfs.Max(X.Degree, Y.Degree, Z.Degree, W.Degree);
    public Vector4 EvalDerivative(float t) => Differentiate().Eval(t);
    public Vector4 EvalSecondDerivative(float t) => Differentiate(2).Eval(t);
    public Vector4 EvalThirdDerivative(float t = 0) => Differentiate(3).Eval(t);
    public Vector4 EvalForthDerivative(float t = 0) => Differentiate(4).Eval(t);

    #endregion

    #region Project Point

    /// <inheritdoc cref="Polynomial2D.ProjectPoint(Vector2,int,int)"/>
    public Vector4 ProjectPoint(Vector4 point, int initialSubdivisions = 16, int refinementIterations = 4) => ProjectPoint(point, out _, initialSubdivisions, refinementIterations);

    static PointProjectSample4D[] pointProjectGuesses = { default, default, default };

    /// <inheritdoc cref="Polynomial2D.ProjectPoint(Vector2,out float,int,int)"/>
    public Vector4 ProjectPoint(Vector4 point, out float t, int initialSubdivisions = 16, int refinementIterations = 4) {
        // define a bezier relative to the test point
        Polynomial4D curve = this;
        curve.X.c0 -= point.X; // constant coefficient defines the start position
        curve.Y.c0 -= point.Y;
        curve.Z.c0 -= point.Z;
        Vector4 curveStart = curve.Eval(0);
        Vector4 curveEnd = curve.Eval(1);

        PointProjectSample4D SampleDistSqDelta(float tSmp) {
            PointProjectSample4D s = new PointProjectSample4D { t = tSmp };
            (s.f, s.fp) = (curve.Eval(tSmp), curve.EvalDerivative(tSmp));
            s.distDeltaSq = Vector4.Dot(s.f, s.fp);
            return s;
        }

        // find initial candidates
        int candidatesFound = 0;
        PointProjectSample4D prevSmp = SampleDistSqDelta(0);

        for (int i = 1; i < initialSubdivisions; i++) {
            float ti = i / (initialSubdivisions - 1f);
            PointProjectSample4D smp = SampleDistSqDelta(ti);
            if (Mathfs.SignAsInt(smp.distDeltaSq) != Mathfs.SignAsInt(prevSmp.distDeltaSq)) {
                pointProjectGuesses[candidatesFound++] = SampleDistSqDelta((prevSmp.t + smp.t) / 2);
                if (candidatesFound == 3) break; // no more than three possible candidates because of the polynomial degree
            }

            prevSmp = smp;
        }

        // refine each guess w. Newton-Raphson iterations
        void Refine(ref PointProjectSample4D smp) {
            Vector4 fpp = curve.EvalSecondDerivative(smp.t);
            float tNew = smp.t - Vector4.Dot(smp.f, smp.fp) / (Vector4.Dot(smp.f, fpp) + Vector4.Dot(smp.fp, smp.fp));
            smp = SampleDistSqDelta(tNew);
        }

        for (int p = 0; p < candidatesFound; p++)
        {
            for (int i = 0; i < refinementIterations; i++)
            {
                Refine(ref pointProjectGuesses[p]);
            }
        }

        // Now find closest. First include the endpoints
        float sqDist0 = curveStart.SqrMagnitude(); // include endpoints
        float sqDist1 = curveEnd.SqrMagnitude();
        bool firstClosest = sqDist0 < sqDist1;
        float tClosest = firstClosest ? 0 : 1;
        Vector4 ptClosest = (firstClosest ? curveStart : curveEnd) + point;
        float distSqClosest = firstClosest ? sqDist0 : sqDist1;

        // then check internal roots
        for (int i = 0; i < candidatesFound; i++) {
            float pSqmag = pointProjectGuesses[i].f.SqrMagnitude();
            if (pSqmag < distSqClosest) {
                distSqClosest = pSqmag;
                tClosest = pointProjectGuesses[i].t;
                ptClosest = pointProjectGuesses[i].f + point;
            }
        }

        t = tClosest;
        return ptClosest;
    }

    #endregion

    #region Typecasting & Operators

    public static Polynomial4D operator /(Polynomial4D p, float v) => new(p.C0 / v, p.C1 / v, p.C2 / v, p.C3 / v);
    public static Polynomial4D operator *(Polynomial4D p, float v) => new(p.C0 * v, p.C1 * v, p.C2 * v, p.C3 * v);
    public static Polynomial4D operator *(float v, Polynomial4D p) => p * v;

    public static explicit operator Vector4Matrix3x1(Polynomial4D poly) => new(poly.C0, poly.C1, poly.C2);
    public static explicit operator Vector4Matrix4x1(Polynomial4D poly) => new(poly.C0, poly.C1, poly.C2, poly.C3);
    public static explicit operator BezierQuad4D(Polynomial4D poly) => poly.Degree < 3 ? new BezierQuad4D(CharacteristicMatrix.QuadraticBezierInverse * (Vector4Matrix3x1)poly) : throw new InvalidCastException("Cannot cast a cubic polynomial to a quadratic curve");
    public static explicit operator BezierCubic4D(Polynomial4D poly) => new(CharacteristicMatrix.CubicBezierInverse * (Vector4Matrix4x1)poly);
    public static explicit operator CatRomCubic4D(Polynomial4D poly) => new(CharacteristicMatrix.CubicCatmullRomInverse * (Vector4Matrix4x1)poly);
    public static explicit operator HermiteCubic4D(Polynomial4D poly) => new(CharacteristicMatrix.CubicHermiteInverse * (Vector4Matrix4x1)poly);
    public static explicit operator UBSCubic4D(Polynomial4D poly) => new(CharacteristicMatrix.CubicUniformBSplineInverse * (Vector4Matrix4x1)poly);

    #endregion
}
