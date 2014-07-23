using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MathNet.Numerics.LinearAlgebra;

namespace ListenPortG4
{
    /// <summary>
    /// A simple override of the MathNet Matrix class so that programs utilizing the Proximity Toolkit do not need to include the
    /// additional reference in order to use Matrix functionality.
    /// </summary>
    [Serializable, Description("Represents a matrix, and provides utility functions for working with matrices.")]
    public class Matrix
    {
        internal MathNet.Numerics.LinearAlgebra.Matrix innermatrix = null;
        private double? det = null;

        #region Constructors
        public Matrix(double[,] A)
        {
            innermatrix = new MathNet.Numerics.LinearAlgebra.Matrix(A);
        }

        public Matrix(double[] vals, int m)
        {
            innermatrix = new MathNet.Numerics.LinearAlgebra.Matrix(vals, m);
        }

        public Matrix(int m, int n)
        {
            innermatrix = new MathNet.Numerics.LinearAlgebra.Matrix(m, n);
        }

        public Matrix(int m, int n, double s)
        {
            innermatrix = new MathNet.Numerics.LinearAlgebra.Matrix(m, n, s);
        }

        public Matrix(Vector3 vector)
        {
            innermatrix = new MathNet.Numerics.LinearAlgebra.Matrix(new double[4, 1] { { vector.X }, { vector.Y }, { vector.Z }, {1.0} });
        }

        internal Matrix(MathNet.Numerics.LinearAlgebra.Matrix innermatrix)
        {
            this.innermatrix = innermatrix;
        }
        #endregion

        #region Properties and Methods
        /// <summary>
        /// Adds two matrices together.
        /// </summary>
        /// <param name="m">The second matrix in the operation.</param>
        [Description("Adds two matrices together.")]
        public void Add(Matrix m)
        {
            innermatrix.Add(m.innermatrix);
            det = null;
        }

        /// <summary>
        /// Performs division of two matrices.
        /// </summary>
        /// <param name="m">The second matrix in the operation.</param>
        [Description("Performs division of two matrices.")]
        public void ArrayDivide(Matrix m)
        {
            innermatrix.ArrayDivide(m.innermatrix);
            det = null;
        }

        /// <summary>
        /// Performs multiplication of two matrices.
        /// </summary>
        /// <param name="m">The second matrix in the operation.</param>
        [Description("Performs multiplication of two matrices.")]
        public void ArrayMultiply(Matrix m)
        {
            innermatrix.ArrayMultiply(m.innermatrix);
            det = null;
        }

        /// <summary>
        /// Makes an exact copy of this matrix as a new object.
        /// </summary>
        /// <returns>The clone of the matrix.</returns>
        [Description("Makes an exact copy of this matrix as a new object.")]
        public Matrix Clone()
        {
            return new Matrix(innermatrix.Clone());
        }

        /// <summary>
        /// Returns the number of columns in this matrix.
        /// </summary>
        [Description("Returns the number of columns in this matrix.")]
        public int ColumnCount
        {
            get
            {
                return innermatrix.ColumnCount;
            }
        }

        [Description("")]
        public double Condition()
        {
            return innermatrix.Condition();
        }

        /// <summary>
        /// Computes the determinant of this matrix.
        /// </summary>
        /// <returns>The determinant of this matrix.</returns>
        [Description("Computes the determinant of this matrix.")]
        public double Determinant()
        {
            return innermatrix.Determinant();
        }

        /// <summary>
        /// Inverts the current matrix.
        /// </summary>
        [Description("Inverts the current matrix.")]
        public void Invert()
        {
            if (det == null) det = Determinant();
            if (det == 0.0) return;
            else innermatrix = innermatrix.Inverse();
        }

        /// <summary>
        /// Returns an inverted clone of this matrix.
        /// </summary>
        /// <returns>An inverted clone of this matrix.</returns>
        [Description("Returns an inverted clone of this matrix.")]
        public Matrix Inverse
        {
            get
            {
                if (det == null) det = Determinant();
                if (det.Value == 0.0) return this;
                else return new Matrix(innermatrix.Inverse());
            }
        }

        /// <summary>
        /// Multiplies a matrix by a scalar value.
        /// </summary>
        /// <param name="s">The scalar value to multiply.</param>
        [Description("Multiplies a matrix by a scalar value.")]
        public void Multiply(double s)
        {
            innermatrix.Multiply(s);
            det = null;
        }

        /// <summary>
        /// Calculates the Rank of the matrix.
        /// </summary>
        /// <returns>The Rank of the matrix.</returns>
        [Description("Calculates the Rank of the matrix.")]
        public int Rank()
        {
            return innermatrix.Rank();
        }

        /// <summary>
        /// Returns the number of rows in this matrix.
        /// </summary>
        [Description("Returns the number of rows in this matrix.")]
        public int RowCount
        {
            get
            {
                return innermatrix.RowCount;
            }
        }

        /// <summary>
        /// Subtracts two matrices.
        /// </summary>
        /// <param name="m">The matrix to subtract.</param>
        [Description("Subtracts two matrices.")]
        public void Subtract(Matrix m)
        {
            innermatrix.Subtract(m.innermatrix);
            det = null;
        }

        /// <summary>
        /// Transposes this matrix.
        /// </summary>
        [Description("Transposes this matrix.")]
        public void Transpose()
        {
            innermatrix.Transpose();
            det = null;
        }

        /// <summary>
        /// Returns a transposed clone of this matrix.
        /// </summary>
        [Description("Returns a transposed clone of this matrix.")]
        public Matrix Transposed
        {
            get
            {
                Matrix m = new Matrix(innermatrix.Clone());
                m.innermatrix.Transpose();
                return m;
            }
        }


        /// <summary>
        /// Performs a Unary Minus operation on this matrix.
        /// </summary>
        [Description("Performs a Unary Minus operation on this matrix.")]
        public void UnaryMinus()
        {
            innermatrix.UnaryMinus();
            det = null;
        }

        /// <summary>
        /// Returns a Unary-Minused clone of this matrix.
        /// </summary>
        [Description("Returns a Unary-Minused clone of this matrix.")]
        public Matrix UnaryMinused
        {
            get
            {
                Matrix m = new Matrix(innermatrix.Clone());
                m.UnaryMinus();
                return m;
            }
        }

        /// <summary>
        /// Gets or sets the value of a single entry in the matrix.
        /// </summary>
        /// <param name="i">The column of the desired entry.</param>
        /// <param name="j">The row of the desired entry.</param>
        /// <returns>The value of the desired entry.</returns>
        [Description("Gets or sets the value of a single entry in the matrix.")]
        public double this[int i, int j]
        {
            get
            {
                return innermatrix[i, j];
            }
            set
            {
                innermatrix[i, j] = value;
                det = null;
            }
        }

        /// <summary>
        /// Converts a columnar matrix to a vector.
        /// </summary>
        /// <returns>The Vector3 equivalent of the matrix.</returns>
        [Description("Converts a columnar matrix to a vector.")]
        public Vector3 ToVector3()
        {
            if (innermatrix.ColumnCount != 1 || innermatrix.RowCount != 4)
                throw new Exception("A " + innermatrix.ColumnCount + "x" + innermatrix.RowCount + " matrix cannot be converted to a Vector3.");

            return new Vector3(innermatrix[0, 0], innermatrix[1, 0], innermatrix[2, 0]);
        }

        /// <summary>
        /// Converts the matrix to a single-dimensional array of doubles.
        /// </summary>
        /// <returns>An array of doubles containing the matrix values.</returns>
        [Description("Converts the matrix to an array of doubles.")]
        public double[] ToArray()
        {
            double[] retval = new double[innermatrix.ColumnCount * innermatrix.RowCount];

            for (int i = 0; i < innermatrix.RowCount; i++)
            {
                for (int j = 0; j < innermatrix.ColumnCount; j++)
                {
                    retval[i * innermatrix.ColumnCount + j] = innermatrix[j, i];
                }
            }

            return retval;
        }

        /// <summary>
        /// Gets only the portion of this matrix representing rotation.
        /// </summary>
        [Description("Gets only the portion of this matrix representing rotation.")]
        public Matrix RotationMatrix
        {
            get
            {
                Matrix retval = this.Clone();
                retval[0, 3] = 0;
                retval[1, 3] = 0;
                retval[2, 3] = 0;
                return retval;
            }
        }

        /// <summary>
        /// Gets only the portion of this matrix representing translation.
        /// </summary>
        [Description("Gets only the portion of this matrix representing translation.")]
        public Matrix TranslationMatrix
        {
            get
            {
                return Matrix.Translation(GetTranslation(this));
            }
        }

        /// <summary>
        /// Applies the transformation represented by this matrix to a vector.
        /// </summary>
        /// <param name="vec">The vector to transform.</param>
        /// <returns>A transformed vector</returns>
        [Description("Applies the transformation represented by this matrix to a vector.")]
        public Vector3 Apply(Vector3 vec)
        {
            return (this * vec.ToMatrix()).ToVector3();
        }

        /// <summary>
        /// Applies the transformation represented by this matrix to an array of vectors.
        /// </summary>
        /// <param name="vecs">The array of vectors to transform.</param>
        /// <returns>A transformed array of vectors.</returns>
        [Description("Applies the transformation represented by this matrix to an array of vectors.")]
        public Vector3[] Apply(Vector3[] vecs)
        {
            Vector3[] retval = new Vector3[vecs.Length];

            for (int i = 0; i < vecs.Length; i++)
            {
                retval[i] = Apply(vecs[i]);
            }

            return retval;
        }

        /// <summary>
        /// Applies the transformation represented by this matrix to a list of vectors.
        /// </summary>
        /// <param name="vecs">The list of vectors to transform.</param>
        /// <returns>A transformed list of vectors.</returns>
        [Description("Applies the transformation represented by this matrix to a list of vectors.")]
        public List<Vector3> Apply(List<Vector3> vecs)
        {
            return new List<Vector3>(Apply(vecs.ToArray()));
        }


        /// <summary>
        /// Applies optionally the rotation and translation represented by this matrix to a list of vectors.
        /// </summary>
        /// <param name="vecs">The list of vectors to transform.</param>
        /// <param name="rotation">Whether or not to apply rotation.</param>
        /// <param name="translation">Whether or not to apply translation.</param>
        /// <returns>A transformed list of vectors.</returns>
        [Description("Applies optionally the rotation and translation represented by this matrix to a list of vectors.")]
        public List<Vector3> Apply(List<Vector3> vecs, bool rotation, bool translation)
        {
            return new List<Vector3>(Apply(vecs.ToArray(), rotation, translation));
        }

        /// <summary>
        /// Applies optionally the rotation and translation represented by this matrix to a vector.
        /// </summary>
        /// <param name="vec">The vector to transform.</param>
        /// <param name="rotation">Whether or not to apply rotation.</param>
        /// <param name="translation">Whether or not to apply translation.</param>
        /// <returns>A transformed vector.</returns>
        [Description("Applies optionally the rotation and translation represented by this matrix to a vector.")]
        public Vector3 Apply(Vector3 vec, bool rotation, bool translation)
        {
            if (rotation && translation) return (this * vec.ToMatrix()).ToVector3();
            else if (rotation) return (RotationMatrix * vec.ToMatrix()).ToVector3();
            else if (translation) return (TranslationMatrix * vec.ToMatrix()).ToVector3();
            else return vec;
        }

        /// <summary>
        /// Applies optionally the rotation and translation represented by this matrix to an array of vectors.
        /// </summary>
        /// <param name="vecs">The array of vectors to transform.</param>
        /// <param name="rotation">Whether or not to apply rotation.</param>
        /// <param name="translation">Whether or not to apply translation.</param>
        /// <returns>A transformed array of vectors.</returns>
        [Description("Applies optionally the rotation and translation represented by this matrix to an array of vectors.")]
        public Vector3[] Apply(Vector3[] vecs, bool rotation, bool translation)
        {
            Vector3[] retval = new Vector3[vecs.Length];

            for (int i = 0; i < vecs.Length; i++)
            {
                retval[i] = Apply(vecs[i], rotation, translation);
            }

            return retval;
        }

        /// <summary>
        /// Transforms the specified vector from the space represented by this matrix, to the specified space.
        /// </summary>
        /// <param name="dest">The destination space.</param>
        /// <param name="vec">The vector to transform.</param>
        /// <returns>A transformed vector.</returns>
        [Description("Transforms the specified vector from the space represented by this matrix, to the specified space.")]
        public Vector3 ToSpace(Matrix dest, Vector3 vec)
        {
            return (Inverse * dest).Apply(vec);
        }

        /// <summary>
        /// Transforms the specified array of vectors from the space represented by this matrix, to the specified space.
        /// </summary>
        /// <param name="dest">The destination space.</param>
        /// <param name="vec">The array of vectors to transform.</param>
        /// <returns>A transformed array of vectors.</returns>
        [Description("Transforms the specified array of vectors from the space represented by this matrix, to the specified space.")]
        public Vector3[] ToSpace(Matrix dest, Vector3[] vecs)
        {
            return (Inverse * dest).Apply(vecs);
        }

        /// <summary>
        /// Transforms the specified list of vectors from the space represented by this matrix, to the specified space.
        /// </summary>
        /// <param name="dest">The destination space.</param>
        /// <param name="vec">The list of vectors to transform.</param>
        /// <returns>A transformed list of vectors.</returns>
        [Description("Transforms the specified list of vectors from the space represented by this matrix, to the specified space.")]
        public List<Vector3> ToSpace(Matrix dest, List<Vector3> vecs)
        {
            return new List<Vector3>((Inverse * dest).Apply(vecs.ToArray()));
        }
        #endregion

        #region Operators
        public static Matrix operator +(Matrix A, Matrix B)
        {
            return new Matrix(A.innermatrix + B.innermatrix);
        }

        public static Matrix operator -(Matrix A, Matrix B)
        {
            return new Matrix(A.innermatrix - B.innermatrix);
        }

        public static Matrix operator *(Matrix A, Matrix B)
        {
            return new Matrix(A.innermatrix * B.innermatrix);
        }

        public static Matrix operator *(double s, Matrix A)
        {
            return new Matrix(s * A.innermatrix);
        }
        #endregion

        #region Object Overrides
        public override bool Equals(object obj)
        {
            if (obj is Matrix)
            {
                return this.innermatrix.Equals(((Matrix)obj).innermatrix);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return innermatrix.GetHashCode();
        }

        /// <summary>
        /// Outputs a string representation of this matrix.
        /// </summary>
        /// <returns>A string representation of this matrix.</returns>
        [Description("Outputs a string representation of this matrix.")]
        public override string ToString()
        {
            string retval = "[";

            for (int i = 0; i < innermatrix.RowCount; i++)
            {
                retval += "[";
                
                for (int j = 0; j < innermatrix.ColumnCount; j++)
                {
                    retval += innermatrix[i, j].ToString("N2") + ", ";
                }

                retval = retval.TrimEnd(new char[] { ' ', ',' }) + "]";
            }

            return retval + "]";
        }

        /// <summary>
        /// Outputs this matrix as a delimited string.
        /// </summary>
        /// <param name="m">The matrix to output.</param>
        /// <param name="coldelim">The delimeter used to separate columns.</param>
        /// <param name="rowdelim">The delimeter used to separate rows.</param>
        /// <returns>A delimited string that represents this matrix.</returns>
        [Description("Outputs this matrix as a delimited string.")]
        public static string ToDelimString(Matrix m, char coldelim, char rowdelim)
        {
            string retval = "";

            for (int j = 0; j < m.RowCount; j++)
            {
                for (int i = 0; i < m.ColumnCount; i++)
                {
                    retval += m[i, j].ToString() + coldelim;
                }

                retval = retval.TrimEnd(coldelim) + rowdelim;
            }

            return retval.TrimEnd(rowdelim);
        }

        /// <summary>
        /// Parses this matrix from a delimited string.
        /// </summary>
        /// <param name="m">The matrix to parse.</param>
        /// <param name="coldelim">The expected delimeter used to separate columns.</param>
        /// <param name="rowdelim">The expected delimeter used to separate rows.</param>
        /// <returns>A matrix parsed from the given string.</returns>
        [Description("Parses this matrix from a delimited string.")]
        public static Matrix FromDelimString(string s, char coldelim, char rowdelim)
        {
            Matrix retval = null;
            string[] rows = s.Split(rowdelim);
            string[] cols = rows[0].Split(coldelim);

            retval = new Matrix(cols.Length, rows.Length);

            for (int j = 0; j < rows.Length; j++)
            {
                cols = rows[j].Split(coldelim);

                for (int i = 0; i < cols.Length; i++)
                {
                    retval[i, j] = double.Parse(cols[i]);
                }
            }

            return retval;
        }
        #endregion

        #region Static Methods
        /// <summary>
        /// Returns an m x m identity matrix.
        /// </summary>
        /// <param name="m">The number of columns/rows of the identity matrix.</param>
        /// <returns>An m x m identity matrix.</returns>
        [Description("Returns an m x m identity matrix.")]
        public static Matrix Identity(int m)
        {
            Matrix identity = new Matrix(m, m, 0);
            for (int i = 0; i < m; i++)
            {
                identity[i, i] = 1;
            }

            return identity;
        }

        /// <summary>
        /// Gets a transformation matrix representing a space based on 3 basis vectors.
        /// </summary>
        /// <param name="xaxis">The x-basis.</param>
        /// <param name="yaxis">The y-basis.</param>
        /// <param name="zaxis">The z-basis.</param>
        /// <returns>A transformation matrix representing the space.</returns>
        [Description("Gets a transformation matrix representing a space based on 3 basis vectors.")]
        public static Matrix Basis(Vector3 xaxis, Vector3 yaxis, Vector3 zaxis)
        {
            return Basis(xaxis, yaxis, zaxis, new Vector3());
        }

        /// <summary>
        /// Gets a transformation matrix representing a space based on 3 basis vectors and a translation vector.
        /// </summary>
        /// <param name="xaxis">The x-basis.</param>
        /// <param name="yaxis">The y-basis.</param>
        /// <param name="zaxis">The z-basis.</param>
        /// <param name="trans">The translation vector.</param>
        /// <returns>A transformation matrix representing the space.</returns>
        [Description("Gets a transformation matrix representing a space based on 3 basis vectors and a translation vector.")]
        public static Matrix Basis(Vector3 xaxis, Vector3 yaxis, Vector3 zaxis, Vector3 trans)
        {
            return new Matrix(new double[,] {
                {xaxis.X, yaxis.X, zaxis.X, trans.X},
                {xaxis.Y, yaxis.Y, zaxis.Y, trans.Y},
                {xaxis.Z, yaxis.Z, zaxis.Z, trans.Z},
                {0, 0, 0, 1}
            });
        }

        /// <summary>
        /// Gets the X-Axis of the transformation represented by a matrix.
        /// </summary>
        /// <param name="m">The matrix.</param>
        /// <returns>The X-Axis.</returns>
        [Description("Gets the X-Axis of the transformation represented by a matrix.")]
        public static Vector3 GetXAxis(Matrix m)
        {
            return new Vector3(m[0, 0], m[1, 0], m[2, 0]);
        }

        /// <summary>
        /// Gets the X-Axis of the transformation represented by this matrix.
        /// </summary>
        /// <returns>The X-Axis.</returns>
        [Description("Gets the X-Axis of the transformation represented by this matrix.")]
        public Vector3 XAxis
        {
            get
            {
                return GetXAxis(this);
            }
        }

        /// <summary>
        /// Gets the Y-Axis of the transformation represented by a matrix.
        /// </summary>
        /// <param name="m">The matrix.</param>
        /// <returns>The Y-Axis.</returns>
        [Description("Gets the Y-Axis of the transformation represented by a matrix.")]
        public static Vector3 GetYAxis(Matrix m)
        {
            return new Vector3(m[0, 1], m[1, 1], m[2, 1]);
        }

        /// <summary>
        /// Gets the Y-Axis of the transformation represented by this matrix.
        /// </summary>
        /// <returns>The Y-Axis.</returns>
        [Description("Gets the Y-Axis of the transformation represented by this matrix.")]
        public Vector3 YAxis
        {
            get
            {
                return GetYAxis(this);
            }
        }

        /// <summary>
        /// Gets the Z-Axis of the transformation represented by a matrix.
        /// </summary>
        /// <param name="m">The matrix.</param>
        /// <returns>The Z-Axis.</returns>
        [Description("Gets the Z-Axis of the transformation represented by a matrix.")]
        public static Vector3 GetZAxis(Matrix m)
        {
            return new Vector3(m[0, 2], m[1, 2], m[2, 2]);
        }

        /// <summary>
        /// Gets the Z-Axis of the transformation represented by this matrix.
        /// </summary>
        /// <returns>The Z-Axis.</returns>
        [Description("Gets the Z-Axis of the transformation represented by this matrix.")]
        public Vector3 ZAxis
        {
            get
            {
                return GetZAxis(this);
            }
        }

        /// <summary>
        /// Gets a matrix transformation representing the rotation of a given angle about a given axis.
        /// </summary>
        /// <param name="angle">The angle of the rotation.</param>
        /// <param name="x">The x-component of the axis.</param>
        /// <param name="y">The y-component of the axis.</param>
        /// <param name="z">The z-component of the axis.</param>
        /// <returns>A matrix transformation representing the rotation.</returns>
        [Description("Gets a matrix transformation representing the rotation of a given angle about a given axis.")]
        public static Matrix AxisRotation(Angle angle, double x, double y, double z)
        {
            return AxisRotation(angle, new Vector3(x, y, z));
        }

        /// <summary>
        /// Gets a matrix transformation representing the rotation of a given angle about a given axis.
        /// </summary>
        /// <param name="angle">The angle of the rotation.</param>
        /// <param name="axis">The axis of the rotation.</param>
        /// <returns></returns>
        [Description("Gets a matrix transformation representing the rotation of a given angle about a given axis.")]
        public static Matrix AxisRotation(Angle angle, Vector3 axis)
        {
            double r = angle.GetValue(AngleUnit.Radians);
            Vector3 u = axis.Normalized;

            double c = Math.Cos(r);
            double s = Math.Sin(r);
            double t = 1 - c;

            Matrix m = new Matrix(new double[,]
                { { u.X * u.X * t + c,
                    u.X * u.Y * t - u.Z * s,
                    u.X * u.Z * t + u.Y * s,
                    0 },
                  { u.X * u.Y * t + u.Z * s,
                    u.Y * u.Y * t + c,
                    u.Y * u.Z * t - u.X * s,
                    0 },
                  { u.X * u.Z * t - u.Y * s,
                    u.Y * u.Z * t + u.X * s,
                    u.Z * u.Z * t + c,
                    0 },
                  { 0, 0, 0, 1 } });

            return m;
        }

        /// <summary>
        /// Gets the translation component of the given matrix as a vector.
        /// </summary>
        /// <param name="m">The matrix to extract the translation from.</param>
        /// <returns>The translation as a vector.</returns>
        [Description("Gets the translation component of the given matrix as a vector.")]
        public static Vector3 GetTranslation(Matrix m)
        {
            Vector3 v = new Vector3();
            v.X = m[0, 3];
            v.Y = m[1, 3];
            v.Z = m[2, 3];

            return v;
        }

        /// <summary>
        /// Sets the translation component of the given matrix from a vector.
        /// </summary>
        /// <param name="m">The matrix to apply the translation to.</param>
        /// <param name="t">The transform vector to apply.</param>
        [Description("Sets the translation component of the given matrix from a vector.")]
        public static void SetTranslation(Matrix m, Vector3 t)
        {
            m[0, 3] = t.X;
            m[1, 3] = t.Y;
            m[2, 3] = t.Z;
        }

        /// <summary>
        /// Finds the Rotation Axis based on a matrix and a known angle.
        /// </summary>
        /// <param name="m">The matrix.</param>
        /// <param name="angle">The known angle.</param>
        /// <returns>The Rotation Axis unit vector.</returns>
        [Description("Finds the Rotation Axis based on a matrix and a known angle.")]
        public static Vector3 GetRotationAxis(Matrix m, Angle angle)
        {
            Vector3 v = new Vector3();

            double c = Math.Cos(angle.GetValue(AngleUnit.Radians));
            double t = 1 - c;

            v.X = Math.Sqrt((m[0, 0] - c) / t);
            v.Y = Math.Sqrt((m[1, 1] - c) / t);
            v.Z = Math.Sqrt((m[2, 2] - c) / t);
            v.Normalize();

            return v;
        }

        /// <summary>
        /// Finds the Rotation Angle based on a matrix and a known axis.
        /// </summary>
        /// <param name="m">The matrix.</param>
        /// <param name="axis">The known axis.</param>
        /// <returns>The Rotation Angle.</returns>
        [Description("Finds the Rotation Angle based on a matrix and a known axis.")]
        public static Angle GetRotationAngle(Matrix m, Vector3 axis)
        {
            Vector3 n = axis.Normalized;
            double x2 = n.X * n.X;
            Angle a = new Angle(
                Math.Acos((m[0, 0] - x2) / (1 - x2)),
                AngleUnit.Radians);

            return a;
        }

        /// <summary>
        /// Gets a translation matrix.
        /// </summary>
        /// <param name="x">The x-component of the tranlation.</param>
        /// <param name="y">The y-component of the tranlation.</param>
        /// <param name="z">The z-component of the tranlation.</param>
        /// <returns></returns>
        [Description("Gets a translation matrix.")]
        public static Matrix Translation(double x, double y, double z)
        {
            return Translation(new Vector3(x, y, z));
        }

        /// <summary>
        /// Gets a translation matrix from a vector.
        /// </summary>
        /// <param name="v">The tranlation as a vector.</param>
        /// <returns>A translation matrix.</returns>
        [Description("Gets a translation matrix from a vector.")]
        public static Matrix Translation(Vector3 v)
        {
            Matrix m = new Matrix( new double[,]
                { { 1, 0, 0, v.X },
                  { 0, 1, 0, v.Y },
                  { 0, 0, 1, v.Z },
                  { 0, 0, 0, 1} } );

            return m;
        }

        /// <summary>
        /// Gets a scale matrix.
        /// </summary>
        /// <param name="x">The x-component of the scale.</param>
        /// <param name="y">The y-component of the scale.</param>
        /// <param name="z">The z-component of the scale.</param>
        /// <returns>A scale matrix.</returns>
        [Description("Gets a scale matrix.")]
        public static Matrix Scale(double x, double y, double z)
        {
            return Scale(new Vector3(x, y, z));
        }

        /// <summary>
        /// Gets a scale matrix from a vector.
        /// </summary>
        /// <param name="v">The scale as a vector.</param>
        /// <returns>A scaled matrix.</returns>
        [Description("Gets a scale matrix from a vector.")]
        public static Matrix Scale(Vector3 v)
        {
            Matrix m = new Matrix(new double[,]
                { { v.X, 0, 0, 0 },
                  { 0, v.Y, 0, 0 },
                  { 0, 0, v.Z, 0 },
                  { 0, 0, 0, 1} });

            return m;
        }

        /// <summary>
        /// Gets the scale factor of each axis as a vector.
        /// </summary>
        /// <param name="m">The matrix from which to extract the scale vector.</param>
        /// <returns>The scale vector.</returns>
        [Description("Gets the scale factor of each axis as a vector.")]
        public static Vector3 GetScale(Matrix m)
        {
            return new Vector3(m[0, 0], m[1, 1], m[2, 2]);
        }

        /// <summary>
        /// Gets a matrix that represents a rotation about the X axis.
        /// </summary>
        /// <param name="angle">Angle in degrees to rotate about X axis.</param>
        /// <returns>A rotation matrix.</returns>
        [Description("Gets a matrix that represents a rotation about the X axis.")]
        public static Matrix RotateX(Angle angle)
        {
            double r = angle.GetValue(AngleUnit.Radians);
            double c = Math.Cos(r);
            double s = Math.Sin(r);

            Matrix m = new Matrix(new double[,]
                { { 1, 0, 0, 0 },
                  { 0, c, -1 * s, 0 },
                  { 0, s, c, 0 },
                  { 0, 0, 0, 1} });

            return m;
        }

        /// <summary>
        /// Gets a matrix that represents a rotation about the Y axis.
        /// </summary>
        /// <param name="angle">Angle in degrees to rotate about Y axis.</param>
        /// <returns>A rotation matrix.</returns>
        [Description("Gets a matrix that represents a rotation about the Y axis.")]
        public static Matrix RotateY(Angle angle)
        {
            double r = angle.GetValue(AngleUnit.Radians);
            double c = Math.Cos(r);
            double s = Math.Sin(r);

            Matrix m = new Matrix(new double[,]
                { { c, 0, s, 0 },
                  { 0, 1, 0, 0 },
                  { -1 * s, 0, c, 0 },
                  { 0, 0, 0, 1} });

            return m;
        }

        /// <summary>
        /// Gets a matrix that represents a rotation about the Z axis.
        /// </summary>
        /// <param name="angle">Angle in degrees to rotate about Z axis.</param>
        /// <returns>A rotation matrix.</returns>
        [Description("Gets a matrix that represents a rotation about the Z axis.")]
        public static Matrix RotateZ(Angle angle)
        {
            double r = angle.GetValue(AngleUnit.Radians);
            double c = Math.Cos(r);
            double s = Math.Sin(r);

            Matrix m = new Matrix(new double[,]
                { { c, -1 * s, 0, 0 },
                  { s, c, 0, 0 },
                  { 0, 0, 1, 0 },
                  { 0, 0, 0, 1} });

            return m;
        }

        /// <summary>
        /// Strips any translation values from a matrix.
        /// </summary>
        /// <param name="M">The matrix to be stripped.</param>
        /// <returns>The stripped matrix.</returns>
        [Description("Removes any translation values from a matrix.")]
        public static Matrix RemoveTranslation(Matrix M)
        {
            Matrix retval = M.Clone();
            retval[0, 3] = 0;
            retval[1, 3] = 0;
            retval[2, 3] = 0;

            return retval;
        }
        #endregion
    }
}
