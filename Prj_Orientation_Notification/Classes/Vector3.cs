#region Imports

using System;
using System.Xml.Serialization;			// for various Xml attributes
using System.Globalization;
using System.Runtime.Serialization;
using System.ComponentModel;
#endregion

/// <summary>
/// Vector of doubles with three components (x,y,z)
/// </summary>
/// <author>Richard Potter BSc(Hons) w/ customizations by Rob Diaz-Marino</author>
/// <created>Jun-04</created>
/// <modified>Jun-10</modified>
/// <version>1.20</version>

namespace ListenPortG4
{
    #region Type Converter
    /// <summary>
    /// Handles converting a Vector3 to a string, and back again.
    /// </summary>
    [Description("Handles converting a Vector3 to a string, and back again.")]
    public class Vector3Converter : StringConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType.Equals(typeof(string))) return true;
            else return false;
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value.GetType() == typeof(string))
            {
                string[] tokens = value.ToString().Split(',');

                try
                {
                    return new Vector3(double.Parse(tokens[0]), double.Parse(tokens[1]), double.Parse(tokens[2]));
                }
                catch
                {
                    throw new InvalidCastException(value.ToString());
                }
            }
            else throw new InvalidCastException(value.ToString());
        }

        public override bool IsValid(ITypeDescriptorContext context, object value)
        {
            return value is Vector3;
        }
    }
    #endregion

    /// <summary>
    /// Represents a 3-tuple vector.
    /// </summary>
    [TypeConverter(typeof(Vector3Converter)), Serializable, Description("Represents a 3-tuple vector.")]
    public struct Vector3
        : IComparable, IComparable<Vector3>, IEquatable<Vector3>, IFormattable
    {
        private double x;
        private double y;
        private double z;
        internal bool notempty;
        public static readonly Vector3 Empty;

        #region Constants
        /// <summary>
        /// The tolerence used when determining the equality of two vectors 
        /// </summary>
        public const double EqualityTolerence = Double.Epsilon;

        /// <summary>
        /// The smallest vector possible (based on the double precision floating point structure)
        /// </summary>
        public static readonly Vector3 MinValue = new Vector3(Double.MinValue, Double.MinValue, Double.MinValue);

        /// <summary>
        /// The largest vector possible (based on the double precision floating point structure)
        /// </summary>
        public static readonly Vector3 MaxValue = new Vector3(Double.MaxValue, Double.MaxValue, Double.MaxValue);

        /// <summary>
        /// The smallest positive (non-zero) vector possible (based on the double precision floating point structure)
        /// </summary>
        public static readonly Vector3 Epsilon = new Vector3(Double.Epsilon, Double.Epsilon, Double.Epsilon);

        /// <summary>
        /// Vector3 representing the Cartesian origin
        /// </summary>
        /// <Acknowledgement>This code is adapted from Exocortex - Ben Houston </Acknowledgement>
        public static readonly Vector3 origin = new Vector3(0, 0, 0);

        /// <summary>
        /// Vector3 representing the Cartesian XAxis
        /// </summary>
        /// <Acknowledgement>This code is adapted from Exocortex - Ben Houston </Acknowledgement>
        public static readonly Vector3 xAxis = new Vector3(1, 0, 0);

        /// <summary>
        /// Vector3 representing the Cartesian YAxis
        /// </summary>
        /// <Acknowledgement>This code is adapted from Exocortex - Ben Houston </Acknowledgement>
        public static readonly Vector3 yAxis = new Vector3(0, 1, 0);

        /// <summary>
        /// Vector3 representing the Cartesian ZAxis
        /// </summary>
        /// <Acknowledgement>This code is adapted from Exocortex - Ben Houston </Acknowledgement>
        public static readonly Vector3 zAxis = new Vector3(0, 0, 1);

        /// <summary>
        /// Exception message descriptive text 
        /// Used for a failure for an array argument to have three components when three are needed 
        /// </summary>
        private const string THREE_COMPONENTS = "Array must contain exactly three components , (x,y,z)";

        /// <summary>
        /// Exception message descriptive text 
        /// Used for a divide by zero event caused by the normalization of a vector with magnitude 0 
        /// </summary>
        private const string NORMALIZE_0 = "Can not normalize a vector when it's magnitude is zero";

        /// <summary>
        /// Exception message descriptive text 
        /// Used when interpolation is attempted with a control parameter not between 0 and 1 
        /// </summary>
        private const string INTERPOLATION_RANGE = "Control parameter must be a value between 0 & 1";

        /// <summary>
        /// Exception message descriptive text 
        /// Used when attempting to compare a Vector3 to an object which is not a type of Vector3 
        /// </summary>
        private const string NON_VECTOR_COMPARISON = "Cannot compare a Vector3 to a non-Vector3";

        /// <summary>
        /// Exception message additional information text 
        /// Used when adding type information of the given argument into an error message 
        /// </summary>
        private const string ARGUMENT_TYPE = "The argument provided is a type of ";

        /// <summary>
        /// Exception message additional information text 
        /// Used when adding value information of the given argument into an error message 
        /// </summary>
        private const string ARGUMENT_VALUE = "The argument provided has a value of ";

        /// <summary>
        /// Exception message additional information text 
        /// Used when adding length (number of components in an array) information of the given argument into an error message 
        /// </summary>
        private const string ARGUMENT_LENGTH = "The argument provided has a length of ";

        /// <summary>
        /// Exception message descriptive text 
        /// Used when attempting to set a Vectors magnitude to a negative value 
        /// </summary>
        private const string NEGATIVE_MAGNITUDE = "The magnitude of a Vector3 must be a positive value, (i.e. greater than 0)";

        /// <summary>
        /// Exception message descriptive text 
        /// Used when attempting to set a Vectors magnitude where the Vector3 represents the origin
        /// </summary>
        private const string ORAGIN_VECTOR_MAGNITUDE = "Cannot change the magnitude of Vector3(0,0,0)";

        ///////////////////////////////////////////////////////////////////////////////

        private const string UNIT_VECTOR = "Unit vector composed of ";

        private const string POSITIONAL_VECTOR = "Positional vector composed of  ";

        private const string MAGNITUDE = " of magnitude ";

        ///////////////////////////////////////////////////////////////////////////////
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor for the Vector3 class accepting three doubles
        /// </summary>
        /// <param name="x">The new x value for the Vector3</param>
        /// <param name="y">The new y value for the Vector3</param>
        /// <param name="z">The new z value for the Vector3</param>
        /// <implementation>
        /// Uses the mutator properties for the Vector3 components to allow verification of input (if implemented)
        /// This results in the need for pre-initialisation initialisation of the Vector3 components to 0 
        /// Due to the necessity for struct's variables to be set in the constructor before moving control
        /// </implementation>
        public Vector3(double x, double y, double z)
        {
            // Pre-initialisation initialisation
            // Implemented because a struct's variables always have to be set in the constructor before moving control
            this.x = 0;
            this.y = 0;
            this.z = 0;
            this.notempty = true;

            // Initialisation
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Constructor for the Vector3 class from an array
        /// </summary>
        /// <param name="xyz">Array representing the new values for the Vector3</param>
        /// <implementation>
        /// Uses the VectorArray property to avoid validation code duplication 
        /// </implementation>
        public Vector3(double[] xyz)
        {
            // Pre-initialisation initialisation
            // Implemented because a struct's variables always have to be set in the constructor before moving control
            this.x = 0;
            this.y = 0;
            this.z = 0;
            this.notempty = true;

            // Initialisation
            Array = xyz;
        }

        /// <summary>
        /// Constructor for the Vector3 class from another Vector3 object
        /// </summary>
        /// <param name="v1">Vector3 representing the new values for the Vector3</param>
        /// <implementation>
        /// Copies values from Vector3 v1 to this vector, does not hold a reference to object v1 
        /// </implementation>
        public Vector3(Vector3 v1)
        {
            // Pre-initialisation initialisation
            // Implemented because a struct's variables always have to be set in the constructor before moving control
            this.x = 0;
            this.y = 0;
            this.z = 0;
            this.notempty = true;

            // Initialisation
            X = v1.X;
            Y = v1.Y;
            Z = v1.Z;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Property for the x component of the Vector3
        /// </summary>
        [Description("X component of the vector.")]
        public double X
        {
            get { return x; }
            set
            {
                x = value;
                notempty = true;
            }
        }

        /// <summary>
        /// Property for the y component of the Vector3
        /// </summary>
        [Description("Y component of the vector.")]
        public double Y
        {
            get { return y; }
            set
            { 
                y = value;
                notempty = true;
            }
        }

        /// <summary>
        /// Property for the z component of the Vector3
        /// </summary>
        [Description("Z component of the vector.")]
        public double Z
        {
            get { return z; }
            set
            {
                z = value;
                notempty = true;
            }
        }

        /// <summary>
        /// Property for the magnitude (aka. length or absolute value) of the Vector3
        /// </summary>
        [Description("The magnitude/length/absolute value of the vector.")]
        public double Magnitude
        {
            get
            {
                return
                Math.Sqrt(SumComponentSqrs());
            }
            set
            {
                if (value < 0)
                { throw new ArgumentOutOfRangeException("value", value, NEGATIVE_MAGNITUDE); }

                if (this == new Vector3(0, 0, 0))
                { throw new ArgumentException(ORAGIN_VECTOR_MAGNITUDE, "this"); }

                this = this * (value / Magnitude);
            }
        }

        /// <summary>
        /// Property for the Vector3 as an array
        /// </summary>
        /// <exception cref="System.ArgumentException">
        /// Thrown if the array argument does not contain exactly three components 
        /// </exception> 
        [XmlIgnore, Description("The vector components as an array.")]
        public double[] Array
        {
            get { return new double[] { x, y, z }; }
            set
            {
                if (value.Length == 3)
                {
                    x = value[0];
                    y = value[1];
                    z = value[2];
                }
                else
                {
                    throw new ArgumentException(THREE_COMPONENTS);
                }
            }
        }

        /// <summary>
        /// An index accessor 
        /// Mapping index [0] -> X, [1] -> Y and [2] -> Z.
        /// </summary>
        /// <param name="index">The array index referring to a component within the vector (i.e. x, y, z)</param>
        /// <exception cref="System.ArgumentException">
        /// Thrown if the array argument does not contain exactly three components 
        /// </exception>
        [Description("Index accessor for vector components. [0]=X, [1]=Y, [2]=Z")]
        public double this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: { return X; }
                    case 1: { return Y; }
                    case 2: { return Z; }
                    default: throw new ArgumentException(THREE_COMPONENTS, "index");
                }
            }
            set
            {
                switch (index)
                {
                    case 0: { X = value; notempty = false; break; }
                    case 1: { Y = value; notempty = false; break; }
                    case 2: { Z = value; notempty = false; break; }
                    default: throw new ArgumentException(THREE_COMPONENTS, "index");
                }
            }
        }

        /// <summary>
        /// The normalized equivalent of this vector.
        /// </summary>
        [Description("The normalized equivalent of this vector.")]
        public Vector3 Normalized
        {
            get
            {
                return Normalize(this);
            }
        }

        /// <summary>
        /// The inverse of this vector, taken by inversing each of its components such that Inverse = (1/X, 1/Y, 1/Z).
        /// </summary>
        [Description("The inverse of this vector, taken by inversing each of its components such that Inverse = (1/X, 1/Y, 1/Z).")]
        public Vector3 Inverse
        {
            get
            {
                Vector3 retval = new Vector3(this);
                retval.X = 1 / retval.X;
                retval.Y = 1 / retval.Y;
                retval.Z = 1 / retval.Z;
                retval.Normalize();

                return retval;
            }
        }

        /// <summary>
        /// The vector with Z component flattened to 0.
        /// </summary>
        [Description("The vector with Z component flattened to 0.")]
        public Vector3 FlattenedXY
        {
            get
            {
                return new Vector3(X, Y, 0);
            }
        }

        /// <summary>
        /// The vector with Y component flattened to 0.
        /// </summary>
        [Description("The vector with Y component flattened to 0.")]
        public Vector3 FlattenedXZ
        {
            get
            {
                return new Vector3(X, 0, Z);
            }
        }

        /// <summary>
        /// The vector with X component flattened to 0.
        /// </summary>
        [Description("The vector with X component flattened to 0.")]
        public Vector3 FlattenedYZ
        {
            get
            {
                return new Vector3(0, Y, Z);
            }
        }

        /// <summary>
        /// The vector with X component flattened to 0.
        /// </summary>
        [Description("Gets the sum of the individual vector components.")]
        public double ComponentSum
        {
            get
            {
                return X + Y + Z;
            }
        }
        #endregion

        #region Operators
        /// <summary>
        /// Addition of two Vectors
        /// </summary>
        /// <param name="v1">Vector3 to be added to </param>
        /// <param name="v2">Vector3 to be added</param>
        /// <returns>Vector3 representing the sum of two Vectors</returns>
        /// <Acknowledgement>This code is adapted from CSOpenGL - Lucas Viñas Livschitz </Acknowledgement>
        public static Vector3 operator +(Vector3 v1, Vector3 v2)
        {
            return
            (
                new Vector3
                    (
                        v1.X + v2.X,
                        v1.Y + v2.Y,
                        v1.Z + v2.Z
                    )
            );
        }

        /// <summary>
        /// Subtraction of two Vectors
        /// </summary>
        /// <param name="v1">Vector3 to be subtracted from </param>
        /// <param name="v2">Vector3 to be subtracted</param>
        /// <returns>Vector3 representing the difference of two Vectors</returns>
        /// <Acknowledgement>This code is adapted from CSOpenGL - Lucas Viñas Livschitz </Acknowledgement>
        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            return
            (
                new Vector3
                    (
                        v1.X - v2.X,
                        v1.Y - v2.Y,
                        v1.Z - v2.Z
                    )
            );
        }

        /// <summary>
        /// Product of a Vector3 and a scalar value
        /// </summary>
        /// <param name="v1">Vector3 to be multiplied </param>
        /// <param name="s2">Scalar value to be multiplied by </param>
        /// <returns>Vector3 representing the product of the vector and scalar</returns>
        /// <Acknowledgement>This code is adapted from CSOpenGL - Lucas Viñas Livschitz </Acknowledgement>
        public static Vector3 operator *(Vector3 v1, double s2)
        {
            return
            (
                new Vector3
                (
                    v1.X * s2,
                    v1.Y * s2,
                    v1.Z * s2
                )
            );
        }

        /// <summary>
        /// Product of a scalar value and a Vector3
        /// </summary>
        /// <param name="s1">Scalar value to be multiplied </param>
        /// <param name="v2">Vector3 to be multiplied by </param>
        /// <returns>Vector3 representing the product of the scalar and Vector3</returns>
        /// <Acknowledgement>This code is adapted from CSOpenGL - Lucas Viñas Livschitz </Acknowledgement>
        /// <Implementation>
        /// Using the commutative law 'scalar x vector'='vector x scalar'.
        /// Thus, this function calls 'operator*(Vector3 v1, double s2)'.
        /// This avoids repetition of code.
        /// </Implementation>
        public static Vector3 operator *(double s1, Vector3 v2)
        {
            return v2 * s1;
        }

        /// <summary>
        /// Division of a Vector3 and a scalar value
        /// </summary>
        /// <param name="v1">Vector3 to be divided </param>
        /// <param name="s2">Scalar value to be divided by </param>
        /// <returns>Vector3 representing the division of the vector and scalar</returns>
        /// <Acknowledgement>This code is adapted from CSOpenGL - Lucas Viñas Livschitz </Acknowledgement>
        public static Vector3 operator /(Vector3 v1, double s2)
        {
            return
            (
                new Vector3
                    (
                        v1.X / s2,
                        v1.Y / s2,
                        v1.Z / s2
                    )
            );
        }

        /// <summary>
        /// Negation of a Vector3
        /// Invert the direction of the Vector3
        /// Make Vector3 negative (-vector)
        /// </summary>
        /// <param name="v1">Vector3 to be negated  </param>
        /// <returns>Negated vector</returns>
        /// <Acknowledgement>This code is adapted from Exocortex - Ben Houston </Acknowledgement>
        public static Vector3 operator -(Vector3 v1)
        {
            return
            (
                new Vector3
                    (
                        -v1.X,
                        -v1.Y,
                        -v1.Z
                    )
            );
        }

        /// <summary>
        /// Reinforcement of a Vector3
        /// Make Vector3 positive (+vector)
        /// </summary>
        /// <param name="v1">Vector3 to be reinforced </param>
        /// <returns>Reinforced vector</returns>
        /// <Acknowledgement>This code is adapted from Exocortex - Ben Houston </Acknowledgement>
        /// <Implementation>
        /// Using the rules of Addition (i.e. '+-x' = '-x' and '++x' = '+x')
        /// This function actually  does nothing but return the argument as given
        /// </Implementation>
        public static Vector3 operator +(Vector3 v1)
        {
            return
            (
                new Vector3
                    (
                        +v1.X,
                        +v1.Y,
                        +v1.Z
                    )
            );
        }

        /// <summary>
        /// Compare the magnitude of two Vectors (less than)
        /// </summary>
        /// <param name="v1">Vector3 to be compared </param>
        /// <param name="v2">Vector3 to be compared with</param>
        /// <returns>True if v1 less than v2</returns>
        public static bool operator <(Vector3 v1, Vector3 v2)
        {
            return v1.SumComponentSqrs() < v2.SumComponentSqrs();
        }

        /// <summary>
        /// Compare the magnitude of two Vectors (greater than)
        /// </summary>
        /// <param name="v1">Vector3 to be compared </param>
        /// <param name="v2">Vector3 to be compared with</param>
        /// <returns>True if v1 greater than v2</returns>
        public static bool operator >(Vector3 v1, Vector3 v2)
        {
            return v1.SumComponentSqrs() > v2.SumComponentSqrs();
        }

        /// <summary>
        /// Compare the magnitude of two Vectors (less than or equal to)
        /// </summary>
        /// <param name="v1">Vector3 to be compared </param>
        /// <param name="v2">Vector3 to be compared with</param>
        /// <returns>True if v1 less than or equal to v2</returns>
        public static bool operator <=(Vector3 v1, Vector3 v2)
        {
            return v1.SumComponentSqrs() <= v2.SumComponentSqrs();
        }

        /// <summary>
        /// Compare the magnitude of two Vectors (greater than or equal to)
        /// </summary>
        /// <param name="v1">Vector3 to be compared </param>
        /// <param name="v2">Vector3 to be compared with</param>
        /// <returns>True if v1 greater than or equal to v2</returns>
        public static bool operator >=(Vector3 v1, Vector3 v2)
        {
            return v1.SumComponentSqrs() >= v2.SumComponentSqrs();
        }

        /// <summary>
        /// Compare two Vectors for equality.
        /// Are two Vectors equal.
        /// </summary>
        /// <param name="v1">Vector3 to be compared for equality </param>
        /// <param name="v2">Vector3 to be compared to </param>
        /// <returns>Boolean decision (truth for equality)</returns>
        /// <implementation>
        /// Checks the equality of each pair of components, all pairs must be equal
        /// A tolerence to the equality operator is applied
        /// </implementation>
        public static bool operator ==(Vector3 v1, Vector3 v2)
        {
            if (v1.notempty != v2.notempty) return false;
            else if (v1.notempty == false) return true;
            else
            {
                return
                (
                    Math.Abs(v1.X - v2.X) <= EqualityTolerence &&
                    Math.Abs(v1.Y - v2.Y) <= EqualityTolerence &&
                    Math.Abs(v1.Z - v2.Z) <= EqualityTolerence
                );
            }
        }

        /// <summary>
        /// Negative comparator of two Vectors.
        /// Are two Vectors different.
        /// </summary>
        /// <param name="v1">Vector3 to be compared for in-equality </param>
        /// <param name="v2">Vector3 to be compared to </param>
        /// <returns>Boolean decision (truth for in-equality)</returns>
        /// <Acknowledgement>This code is adapted from CSOpenGL - Lucas Viñas Livschitz </Acknowledgement>
        /// <implementation>
        /// Uses the equality operand function for two vectors to prevent code duplication
        /// </implementation>
        public static bool operator !=(Vector3 v1, Vector3 v2)
        {
            return !(v1 == v2);
        }

        #endregion

        #region Methods
        /// <summary>
        /// Determine the cross product of two Vectors
        /// Determine the vector product
        /// Determine the normal vector (Vector3 90° to the plane)
        /// </summary>
        /// <param name="v1">The vector to multiply</param>
        /// <param name="v2">The vector to multiply by</param>
        /// <returns>Vector3 representing the cross product of the two vectors</returns>
        /// <implementation>
        /// Cross products are non commutable
        /// </implementation>
        /// <Acknowledgement>This code is adapted from CSOpenGL - Lucas Viñas Livschitz </Acknowledgement>
        [Description("Calculate the cross product of two vectors, which results in a vector 90° to both.")]
        public static Vector3 CrossProduct(Vector3 v1, Vector3 v2)
        {
            return
            (
                new Vector3
                (
                    v1.Y * v2.Z - v1.Z * v2.Y,
                    v1.Z * v2.X - v1.X * v2.Z,
                    v1.X * v2.Y - v1.Y * v2.X
                )
            );
        }

        /// <summary>
        /// Determine the cross product of this Vector3 and another
        /// Determine the vector product
        /// Determine the normal vector (Vector3 90° to the plane)
        /// </summary>
        /// <param name="other">The vector to multiply by</param>
        /// <returns>Vector3 representing the cross product of the two vectors</returns>
        /// <implementation>
        /// Uses the CrossProduct function to avoid code duplication
        /// <see cref="CrossProduct(Vector3, Vector3)"/>
        /// </implementation>
        [Description("Calculate the cross product of two vectors, which results in a vector 90° to both.")]
        public Vector3 CrossProduct(Vector3 other)
        {
            return CrossProduct(this, other);
        }

        /// <summary>
        /// Determine the dot product of two Vectors
        /// </summary>
        /// <param name="v1">The vector to multiply</param>
        /// <param name="v2">The vector to multiply by</param>
        /// <returns>Scalar representing the dot product of the two vectors</returns>
        /// <implementation>
        /// </implementation>
        /// <Acknowledgement>This code is adapted from CSOpenGL - Lucas Viñas Livschitz </Acknowledgement>
        [Description("Calculate the dot product of two vectors, which results in a scalar value that is useful for calculating angles and projections.")]
        public static double DotProduct(Vector3 v1, Vector3 v2)
        {
            return
            (
                v1.X * v2.X +
                v1.Y * v2.Y +
                v1.Z * v2.Z
            );
        }

        /// <summary>
        /// Determine the dot product of this Vector3 and another
        /// </summary>
        /// <param name="other">The vector to multiply by</param>
        /// <returns>Scalar representing the dot product of the two vectors</returns>
        /// <implementation>
        /// <see cref="DotProduct(Vector3)"/>
        /// </implementation>
        [Description("Calculate the dot product of two vectors, which results in a scalar value that is useful for calculating angles and projections.")]
        public double DotProduct(Vector3 other)
        {
            return DotProduct(this, other);
        }

        /// <summary>
        /// Determine the mixed product of three Vectors
        /// Determine volume (with sign precision) of parallelepiped spanned on given vectors
        /// Determine the scalar triple product of three vectors
        /// </summary>
        /// <param name="v1">The first vector</param>
        /// <param name="v2">The second vector</param>
        /// <param name="v3">The third vector</param>
        /// <returns>Scalar representing the mixed product of the three vectors</returns>
        /// <implementation>
        /// Mixed products are non commutable
        /// <see cref="CrossProduct(Vector3, Vector3)"/>
        /// <see cref="DotProduct(Vector3, Vector3)"/>
        /// </implementation>
        /// <Acknowledgement>This code was provided by Michał Bryłka</Acknowledgement>
        [Description("Calculates the mixed product of three vectors, which equals the volume of the parallelopiped formed by those vectors.")]
        public static double MixedProduct(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            return DotProduct(CrossProduct(v1, v2), v3);
        }

        /// <summary>
        /// Determine the mixed product of three Vectors
        /// Determine volume (with sign precision) of parallelepiped spanned on given vectors
        /// Determine the scalar triple product of three vectors
        /// </summary>
        /// <param name="other_v1">The second vector</param>
        /// <param name="other_v2">The third vector</param>
        /// <returns>Scalar representing the mixed product of the three vectors</returns>
        /// <implementation>
        /// Mixed products are non commutable
        /// <see cref="MixedProduct(Vector3, Vector3, Vector3)"/>
        /// Uses MixedProduct(Vector3, Vector3, Vector3) to avoid code duplication
        /// </implementation>
        [Description("Calculates the mixed product of three vectors, which equals the volume of the parallelopiped formed by those vectors.")]
        public double MixedProduct(Vector3 other_v1, Vector3 other_v2)
        {
            return DotProduct(CrossProduct(this, other_v1), other_v2);
        }

        /// <summary>
        /// Get the normalized vector
        /// Get the unit vector
        /// Scale the Vector3 so that the magnitude is 1
        /// </summary>
        /// <param name="v1">The vector to be normalized</param>
        /// <returns>The normalized Vector3</returns>
        /// <implementation>
        /// Uses the Magnitude function to avoid code duplication 
        /// </implementation>
        /// <exception cref="System.DivideByZeroException">
        /// Thrown when the normalisation of a zero magnitude vector is attempted
        /// </exception>
        /// <Acknowledgement>This code is adapted from Exocortex - Ben Houston </Acknowledgement>
        [Description("Normalizes a vector such that the magnitude is 1, but the components are scaled to maintain their relative proportions.")]
        public static Vector3 Normalize(Vector3 v1)
        {
            // Check for divide by zero errors
            if (v1.Magnitude == 0)
            {
                return v1;
            }
            else
            {
                // find the inverse of the vectors magnitude
                double inverse = 1 / v1.Magnitude;
                return
                (
                    new Vector3
                    (
                    // multiply each component by the inverse of the magnitude
                        v1.X * inverse,
                        v1.Y * inverse,
                        v1.Z * inverse
                    )
                );
            }
        }

        /// <summary>
        /// Get the normalized vector
        /// Get the unit vector
        /// Scale the Vector3 so that the magnitude is 1
        /// </summary>
        /// <returns>The normalized Vector3</returns>
        /// <implementation>
        /// Uses the Magnitude and Normalize function to avoid code duplication 
        /// </implementation>
        /// <exception cref="System.DivideByZeroException">
        /// Thrown when the normalisation of a zero magnitude vector is attempted
        /// </exception>
        /// <Acknowledgement>This code is adapted from Exocortex - Ben Houston </Acknowledgement>
        [Description("Normalizes this vector such that the magnitude is 1, but the components are scaled to maintain their relative proportions.")]
        public void Normalize()
        {
            this = Normalize(this);
        }

        /// <summary>
        /// Returns a zero vector.
        /// </summary>
        [Description("Returns a zero vector.")]
        public void Zero()
        {
            x = 0;
            y = 0;
            z = 0;
        }

        /// <summary>
        /// Take an interpolated value from between two Vectors or an extrapolated value if allowed
        /// </summary>
        /// <param name="v1">The Vector3 to interpolate from (where control ==0)</param>
        /// <param name="v2">The Vector3 to interpolate to (where control ==1)</param>
        /// <param name="control">The interpolated point between the two vectors to retrieve (fraction between 0 and 1), or an extrapolated point if allowed</param>
        /// <param name="allowExtrapolation">True if the control may represent a point not on the vertex between v1 and v2</param>
        /// <returns>The value at an arbitrary distance (interpolation) between two vectors or an extrapolated point on the extended virtex</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when the control is not between values of 0 and 1 and extrapolation is not allowed
        /// </exception>
        /// <Acknowledgement>This code is adapted from Exocortex - Ben Houston </Acknowledgement>
        [Description("Take an interpolated value from between two vectors, or an extrapolated value if allowed.")]
        public static Vector3 Interpolate(Vector3 v1, Vector3 v2, double control, bool allowExtrapolation)
        {
            if (!allowExtrapolation && (control > 1 || control < 0))
            {
                // Error message includes information about the actual value of the argument
                throw new ArgumentOutOfRangeException
                        (
                            "control",
                            control,
                            INTERPOLATION_RANGE + "\n" + ARGUMENT_VALUE + control
                        );
            }
            else
            {
                return
                (
                    new Vector3
                    (
                        v1.X * (1 - control) + v2.X * control,
                        v1.Y * (1 - control) + v2.Y * control,
                        v1.Z * (1 - control) + v2.Z * control
                    )
                );
            }
        }

        /// <summary>
        /// Take an interpolated value from between two Vectors
        /// </summary>
        /// <param name="v1">The Vector3 to interpolate from (where control ==0)</param>
        /// <param name="v2">The Vector3 to interpolate to (where control ==1)</param>
        /// <param name="control">The interpolated point between the two vectors to retrieve (fraction between 0 and 1)</param>
        /// <returns>The value at an arbitrary distance (interpolation) between two vectors</returns>
        /// <implementation>
        /// <see cref="Interpolate(Vector3, Vector3, double, bool)"/>
        /// Uses the Interpolate(Vector3,Vector3,double,bool) method to avoid code duplication
        /// </implementation>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when the control is not between values of 0 and 1
        /// </exception>
        /// <Acknowledgement>This code is adapted from Exocortex - Ben Houston </Acknowledgement>
        [Description("Take an interpolated value from between two vectors.")]
        public static Vector3 Interpolate(Vector3 v1, Vector3 v2, double control)
        {
            return Interpolate(v1, v2, control, false);
        }


        /// <summary>
        /// Take an interpolated value from between two Vectors
        /// </summary>
        /// <param name="other">The Vector3 to interpolate to (where control ==1)</param>
        /// <param name="control">The interpolated point between the two vectors to retrieve (fraction between 0 and 1)</param>
        /// <returns>The value at an arbitrary distance (interpolation) between two vectors</returns>
        /// <implementation>
        /// <see cref="Interpolate(Vector3, Vector3, double)"/>
        /// Overload for Interpolate method, finds an interpolated value between this Vector3 and another
        /// Uses the Interpolate(Vector3,Vector3,double) method to avoid code duplication
        /// </implementation>
        [Description("Take an interpolated value from between two vectors.")]
        public Vector3 Interpolate(Vector3 other, double control)
        {
            return Interpolate(this, other, control);
        }

        /// <summary>
        /// Take an interpolated value from between two Vectors or an extrapolated value if allowed
        /// </summary>
        /// <param name="other">The Vector3 to interpolate to (where control ==1)</param>
        /// <param name="control">The interpolated point between the two vectors to retrieve (fraction between 0 and 1), or an extrapolated point if allowed</param>
        /// <param name="allowExtrapolation">True if the control may represent a point not on the vertex between v1 and v2</param>
        /// <returns>The value at an arbitrary distance (interpolation) between two vectors or an extrapolated point on the extended virtex</returns>
        /// <implementation>
        /// <see cref="Interpolate(Vector3, Vector3, double, bool)"/>
        /// Uses the Interpolate(Vector3,Vector3,double,bool) method to avoid code duplication
        /// </implementation>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when the control is not between values of 0 and 1 and extrapolation is not allowed
        /// </exception>
        [Description("Take an interpolated value from between two vectors, or an extrapolated value if allowed.")]
        public Vector3 Interpolate(Vector3 other, double control, bool allowExtrapolation)
        {
            return Interpolate(this, other, control);
        }

        /// <summary>
        /// Find the distance between two Vectors
        /// Pythagoras theorem on two Vectors
        /// </summary>
        /// <param name="v1">The Vector3 to find the distance from </param>
        /// <param name="v2">The Vector3 to find the distance to </param>
        /// <returns>The distance between two Vectors</returns>
        /// <implementation>
        /// </implementation>
        [Description("Find the distance between two vectors.")]
        public static double Distance(Vector3 v1, Vector3 v2)
        {
            return
            (
                Math.Sqrt
                (
                    (v1.X - v2.X) * (v1.X - v2.X) +
                    (v1.Y - v2.Y) * (v1.Y - v2.Y) +
                    (v1.Z - v2.Z) * (v1.Z - v2.Z)
                )
            );
        }

        /// <summary>
        /// Find the distance between two Vectors
        /// Pythagoras theorem on two Vectors
        /// </summary>
        /// <param name="other">The Vector3 to find the distance to </param>
        /// <returns>The distance between two Vectors</returns>
        /// <implementation>
        /// <see cref="Distance(Vector3, Vector3)"/>
        /// Overload for Distance method, finds distance between this Vector3 and another
        /// Uses the Distance(Vector3,Vector3) method to avoid code duplication
        /// </implementation>
        [Description("Find the distance between this vector and another.")]
        public double Distance(Vector3 other)
        {
            return Distance(this, other);
        }

        /// <summary>
        /// Finds the positive symmetric angle (0° to 180°) between two vectors in either direction.
        /// </summary>
        /// <param name="v1">The vector to discern the angle from.</param>
        /// <param name="v2">The vector to discern the angle to.</param>
        /// <returns>The angle between the two vectors.</returns>
        /// <implementation>
        /// </implementation>
        [Description("Find the positive symmetric angle (0° to 180°) between two vectors in either direction.")]
        public static Angle AnglePS(Vector3 v1, Vector3 v2)
        {
            double angleval = Math.Acos(Math.Round(Normalize(v1).DotProduct(Normalize(v2)), 8));
            if (angleval == double.NaN) return Angle.Angle0Degrees;
            else return new Angle(angleval, AngleUnit.Radians);
        }

        /// <summary>
        /// Finds the positive angle (0° to 360°) between two vectors about a normal axis.
        /// </summary>
        /// <param name="v1">The vector to discern the angle from.</param>
        /// <param name="v2">The vector to discern the angle to.</param>
        /// <param name="axis">The normal axis about which the angle occurs.</param>
        /// <returns>The angle between the two vectors.</returns>
        [Description("Find the positive angle (0° to 360°) between two vectors about a normal axis.")]
        public static Angle AngleP(Vector3 v1, Vector3 v2, Vector3 axis)
        {
            if (v1.Magnitude == 0 || v2.Magnitude == 0) return new Angle();
            Angle retval = AnglePS(v1, v2);
            if (v1.CrossProduct(v2).Magnitude == 0) return retval;

            // Establish a basis
            Vector3 x = v1.Normalized;
            Vector3 y = axis.Normalized;
            Vector3 z = x.CrossProduct(y);
            if (z.Magnitude == 0) return retval;  // Parallel

            Angle zv = AnglePS(z, v2);

            if (zv.Radians > Math.PI / 2) return new Angle(2 * Math.PI - retval.Radians, AngleUnit.Radians);
            else return retval;
        }

        /// <summary>
        /// Finds the symmetric angle (-180° to 180°) between two vectors about a normal axis.
        /// </summary>
        /// <param name="v1">The vector to discern the angle from.</param>
        /// <param name="v2">The vector to discern the angle to.</param>
        /// <param name="axis">The normal axis about which the angle occurs.</param>
        /// <returns>The angle between the two vectors.</returns>
        [Description("Find the symmetric angle (-180° to 180°) between two vectors about a normal axis.")]
        public static Angle AngleS(Vector3 v1, Vector3 v2, Vector3 axis)
        {
            if (v1.Magnitude == 0 || v2.Magnitude == 0) return new Angle();
            Angle retval = AnglePS(v1, v2);
            if (v1.CrossProduct(v2).Magnitude == 0) return retval;

            // Establish a basis
            Vector3 x = v1.Normalized;
            Vector3 y = axis.Normalized;
            Vector3 z = x.CrossProduct(y);
            if (z.Magnitude == 0) return retval;  // Parallel

            Angle zv = AnglePS(z, v2);

            if (zv.Radians > Math.PI / 2) return -1 * retval;
            else return retval;
        }


        /// <summary>
        /// Find the positive symmetric angle (0° to 180°) between this and another vector in either direction.
        /// </summary>
        /// <param name="other">The vector to discern the angle to.</param>
        /// <returns>The positive symmetric angle between two vectors.</returns>
        [Description("Find the positive symmetric angle (0° to 180°) between this and another vector in either direction.")]
        public Angle AnglePS(Vector3 other)
        {
            return AnglePS(this, other);
        }

        /// <summary>
        /// Find the positive angle (0° to 360°) between this and another vector about a normal axis.
        /// </summary>
        /// <param name="other">The vector to discern the angle to.</param>
        /// <param name="axis">The normal axis about which the angle occurs.</param>
        /// <returns></returns>
        [Description("Find the positive angle (0° to 360°) between this and another vector about a normal axis.")]
        public Angle AngleP(Vector3 other, Vector3 axis)
        {
            return AngleP(this, other, axis);
        }

        /// <summary>
        /// Find the symmetric angle (-180° to 180°) between this and another vector about a normal axis.
        /// </summary>
        /// <param name="other">The vector to discern the angle to.</param>
        /// <param name="axis">The normal axis about which the angle occurs.</param>
        /// <returns></returns>
        [Description("Find the symmetric angle (-180° to 180°) between this and another vector about a normal axis.")]
        public Angle AngleS(Vector3 other, Vector3 axis)
        {
            return AngleS(this, other, axis);
        }

        /// <summary>
        /// Compares the magnitude of two vectors and returns the greater.
        /// </summary>
        /// <param name="v1">The vector to compare</param>
        /// <param name="v2">The vector to compare with</param>
        /// <returns>
        /// The greater of the two Vectors (based on magnitude)
        /// </returns>
        /// <Acknowledgement>This code is adapted from Exocortex - Ben Houston </Acknowledgement>
        [Description("Compares the magnitude of two vectors and returns the greater.")]
        public static Vector3 Max(Vector3 v1, Vector3 v2)
        {
            if (v1 >= v2) { return v1; }
            return v2;
        }

        /// <summary>
        /// Compares the magnitude of this and another vector and returns the greater.
        /// </summary>
        /// <param name="other">The vector to compare with</param>
        /// <returns>
        /// The greater of the two Vectors (based on magnitude)
        /// </returns>
        /// <implementation>
        /// <see cref="Max(Vector3, Vector3)"/>
        /// Uses function Max(Vector3, Vector3) to avoid code duplication
        /// </implementation>
        [Description("Compares the magnitude of this and another vector and returns the greater.")]
        public Vector3 Max(Vector3 other)
        {
            return Max(this, other);
        }

        /// <summary>
        /// Compares the magnitude of two vectors and returns the lesser.
        /// </summary>
        /// <param name="v1">The vector to compare</param>
        /// <param name="v2">The vector to compare with</param>
        /// <returns>
        /// The lesser of the two Vectors (based on magnitude)
        /// </returns>
        /// <Acknowledgement>This code is adapted from Exocortex - Ben Houston </Acknowledgement>
        [Description("Compares the magnitude of two vectors and returns the lesser.")]
        public static Vector3 Min(Vector3 v1, Vector3 v2)
        {
            if (v1 <= v2) { return v1; }
            return v2;
        }

        /// <summary>
        /// Compares the magnitude of this and another vector and returns the lesser.
        /// </summary>
        /// <param name="other">The vector to compare with</param>
        /// <returns>
        /// The lesser of the two Vectors (based on magnitude)
        /// </returns>
        /// <implementation>
        /// <see cref="Min(Vector3, Vector3)"/>
        /// Uses function Min(Vector3, Vector3) to avoid code duplication
        /// </implementation>
        [Description("Compares the magnitude of this and another vector and returns the lesser.")]
        public Vector3 Min(Vector3 other)
        {
            return Min(this, other);
        }

        /// <summary>
        /// Rotates a vector around the Y axis.
        /// Change the yaw of a Vector3
        /// </summary>
        /// <param name="v1">The Vector3 to be rotated</param>
        /// <param name="degree">The angle to rotate the Vector3 around in degrees</param>
        /// <returns>Vector3 representing the rotation around the Y axis</returns>
        [Description("Rotates a vector around the Y axis.")]
        public static Vector3 Yaw(Vector3 v1, double degree)
        {
            double x = (v1.Z * Math.Sin(degree)) + (v1.X * Math.Cos(degree));
            double y = v1.Y;
            double z = (v1.Z * Math.Cos(degree)) - (v1.X * Math.Sin(degree));
            return new Vector3(x, y, z);
        }

        /// <summary>
        /// Rotates this vector around the Y axis.
        /// Change the yaw of the Vector3
        /// </summary>
        /// <param name="degree">The angle to rotate the Vector3 around in degrees</param>
        /// <returns>Vector3 representing the rotation around the Y axis</returns>
        /// <implementation>
        /// <see cref="Yaw(Vector3, Double)"/>
        /// Uses function Yaw(Vector3, double) to avoid code duplication
        /// </implementation>
        [Description("Rotates this vector around the Y axis.")]
        public void Yaw(double degree)
        {
            this = Yaw(this, degree);
        }

        /// <summary>
        /// Rotates a vector around the X axis.
        /// Change the pitch of a Vector3
        /// </summary>
        /// <param name="v1">The Vector3 to be rotated</param>
        /// <param name="degree">The angle to rotate the Vector3 around in degrees</param>
        /// <returns>Vector3 representing the rotation around the X axis</returns>
        [Description("Rotates a vector around the X axis.")]
        public static Vector3 Pitch(Vector3 v1, double degree)
        {
            double x = v1.X;
            double y = (v1.Y * Math.Cos(degree)) - (v1.Z * Math.Sin(degree));
            double z = (v1.Y * Math.Sin(degree)) + (v1.Z * Math.Cos(degree));
            return new Vector3(x, y, z);
        }

        /// <summary>
        /// Rotates this vector around the X axis.
        /// Change the pitch of a Vector3
        /// </summary>
        /// <param name="degree">The angle to rotate the Vector3 around in degrees</param>
        /// <returns>Vector3 representing the rotation around the X axis</returns>
        /// <see cref="Pitch(Vector3, Double)"/>
        /// <implementation>
        /// Uses function Pitch(Vector3, double) to avoid code duplication
        /// </implementation>
        [Description("Rotates this vector around the X axis.")]
        public void Pitch(double degree)
        {
            this = Pitch(this, degree);
        }

        /// <summary>
        /// Rotates a vector around the Z axis.
        /// Change the roll of a Vector3
        /// </summary>
        /// <param name="v1">The Vector3 to be rotated</param>
        /// <param name="degree">The angle to rotate the Vector3 around in degrees</param>
        /// <returns>Vector3 representing the rotation around the Z axis</returns>
        [Description("Rotates a vector around the Z axis.")]
        public static Vector3 Roll(Vector3 v1, double degree)
        {
            double x = (v1.X * Math.Cos(degree)) - (v1.Y * Math.Sin(degree));
            double y = (v1.X * Math.Sin(degree)) + (v1.Y * Math.Cos(degree));
            double z = v1.Z;
            return new Vector3(x, y, z);
        }

        /// <summary>
        /// Rotates this vector around the Z axis.
        /// Change the roll of a Vector3
        /// </summary>
        /// <param name="degree">The angle to rotate the Vector3 around in degrees</param>
        /// <returns>Vector3 representing the rotation around the Z axis</returns>
        /// <implementation>
        /// <see cref="Roll(Vector3, Double)"/>
        /// Uses function Roll(Vector3, double) to avoid code duplication
        /// </implementation>
        [Description("Rotates this vector around the Z axis.")]
        public void Roll(double degree)
        {
            this = Roll(this, degree);
        }

        /// <summary>
        /// Finds the absolute value of a vector.
        /// Find the magnitude of a Vector3
        /// </summary>
        /// <returns>A Vector3 representing the absolute values of the vector</returns>
        /// <implementation>
        /// An alternative interface to the magnitude property
        /// </implementation>
        [Description("Finds the absolute value of a vector.")]
        public static double Abs(Vector3 v1)
        {
            return v1.Magnitude;
        }

        /// <summary>
        /// Find the absolute value of this vector.
        /// Find the magnitude of a Vector3
        /// </summary>
        /// <returns>A Vector3 representing the absolute values of the vector</returns>
        /// <implementation>
        /// An alternative interface to the magnitude property
        /// </implementation>
        [Description("Finds the absolute value of this vector.")]
        public double Abs()
        {
            return this.Magnitude;
        }

        /// <summary>
        /// Finds the mid point between this and another vector.
        /// </summary>
        /// <param name="other">The other vector.</param>
        /// <returns>The mid point between vectors.</returns>
        [Description("Finds the mid point between this and another vector.")]
        public Vector3 MidPoint(Vector3 other)
        {
            return Vector3.MidPoint(this, other);
        }

        /// <summary>
        /// Finds the mid point between two vectors.
        /// </summary>
        /// <param name="v1">The first vector.</param>
        /// <param name="v2">The second vector.</param>
        /// <returns>The mid point between vectors.</returns>
        [Description("Finds the mid point between two vectors.")]
        public static Vector3 MidPoint(Vector3 v1, Vector3 v2)
        {
            return v1 + ((v2 - v1) / 2);
        }

        /// <summary>
        /// Converts this vector to a columnar matrix.
        /// </summary>
        /// <returns></returns>
        [Description("Converts this vector to a columnar matrix.")]
        public Matrix ToMatrix()
        {
            return new Matrix(this);
        }

        /// <summary>
        /// Rounds the components of th evector to a certain decimal place.
        /// </summary>
        /// <param name="decimalplaces"></param>
        /// <returns></returns>
        [Description("Returns a copy of this vector with the components rounded to a certain decimal place.")]
        public Vector3 Rounded(int decimalplaces)
        {
            return new Vector3( Math.Round(X, decimalplaces), Math.Round(Y, decimalplaces), Math.Round(Z, decimalplaces));
        }

        /// <summary>
        /// Rounds the components of this vector to a certain decimal place.
        /// </summary>
        /// <param name="decimalplaces"></param>
        [Description("Rounds the components of this vector to a certain decimal place.")]
        public void Round(int decimalplaces)
        {
            X = Math.Round(X, decimalplaces);
            Y = Math.Round(Y, decimalplaces);
            Z = Math.Round(Z, decimalplaces);
        }

        /// <summary>
        /// Gets this vector with components overwritten by zero values from the mask.
        /// </summary>
        /// <param name="mask">The mask.</param>
        /// <returns>The masked vector.</returns>
        [Description("Gets this vector with components overwritten by zero values from the mask.")]
        public Vector3 GetMasked(Vector3 mask)
        {
            return new Vector3(mask.X == 0 ? 0 : X, mask.Y == 0 ? 0 : Y, mask.Z == 0 ? 0 : Z);
        }

        /// <summary>
        /// Gets this vector with components affected by the sign of the mask, and zero values overwritten.
        /// </summary>
        /// <param name="mask">The mask.</param>
        /// <returns>The masked vector.</returns>
        [Description("Gets this vector with components affected by the sign of the mask, and zero values overwritten.")]
        public Vector3 GetSignMasked(Vector3 mask)
        {
            return new Vector3(
                mask.X == 0 ? 0 : X * Math.Sign(mask.X),
                mask.Y == 0 ? 0 : Y * Math.Sign(mask.Y),
                mask.Z == 0 ? 0 : Z * Math.Sign(mask.Z));
        }

        /// <summary>
        /// Gets the Incline/Azimuth/Roll relative to a world basis.
        /// </summary>
        /// <param name="worldfront">The world forward direction, or primary horizontal axis.</param>
        /// <param name="worldup">The world up direction, or positive vertical axis.</param>
        /// <param name="worldlong">A lateral world vector, purpendicular to both world front and world up.</param>
        /// <param name="front">The front vector of an object.</param>
        /// <param name="up">The up vector of an object.</param>
        /// <param name="incline">The calculated incline.</param>
        /// <param name="azimuth">The calculated azimuth.</param>
        /// <param name="roll">The calculated roll.</param>
        [Description("Gets the Incline/Azimuth/Roll assuming an XY ground plane.")]
        public static void GetInclineAzimuthRoll(Vector3 worldfront, Vector3 worldup, Vector3 worldlat, Vector3 front, Vector3 up, out Angle incline, out Angle azimuth, out Angle roll)
        {
            // Calculate the model basis
            front = front.Normalized;
            Vector3 ground = worldfront + worldlat;
            Vector3 lateral = front.CrossProduct(up).Normalized;
            up = front.CrossProduct(lateral).Normalized;
            Vector3 frontflat = front.GetMasked(ground);
            Vector3 abslat = front.CrossProduct(worldup);
            Vector3 absup = abslat.CrossProduct(front);

            // Calculate the final angles
            incline = frontflat.AnglePS(front) * Math.Sign(-1 * front.GetSignMasked(worldup).ComponentSum);
            azimuth = -1 * worldfront.AngleS(frontflat, worldup);
            roll = absup.AngleS(-1 * up, front);
        }
        #endregion

        #region Component Operations
        /// <summary>
        /// The sum of a vector's components.
        /// </summary>
        /// <param name="v1">The vector whose scalar components to sum.</param>
        /// <returns>The sum of the Vectors X, Y and Z components.</returns>
        [Description("The sum of a vector's components.")]
        public static double SumComponents(Vector3 v1)
        {
            return (v1.X + v1.Y + v1.Z);
        }

        /// <summary>
        /// The sum of this vector's components.
        /// </summary>
        /// <returns>The sum of the Vectors X, Y and Z components.</returns>
        /// <implementation>
        /// <see cref="SumComponents(Vector3)"/>
        /// The Components.SumComponents(Vector3) function has been used to prevent code duplication.
        /// </implementation>
        [Description("The sum of this vector's components.")]
        public double SumComponents()
        {
            return SumComponents(this);
        }

        /// <summary>
        /// The sum of a vector's squared components.
        /// </summary>
        /// <param name="v1">The vector whose scalar components to square and sum.</param>
        /// <returns>The sum of the Vectors X^2, Y^2 and Z^2 components.</returns>
        [Description("The sum of a vector's squared components.")]
        public static double SumComponentSqrs(Vector3 v1)
        {
            Vector3 v2 = SqrComponents(v1);
            return v2.SumComponents();
        }

        /// <summary>
        /// The sum of this vector's squared components
        /// </summary>
        /// <returns>The sum of the Vectors X^2, Y^2 and Z^2 components</returns>
        /// <implementation>
        /// <see cref="SumComponentSqrs(Vector3)"/>
        /// The Components.SumComponentSqrs(Vector3) function has been used to prevent code duplication
        /// </implementation>
        [Description("The sum of this vector's squared components.")]
        public double SumComponentSqrs()
        {
            return SumComponentSqrs(this);
        }

        /// <summary>
        /// The individual multiplication to a power of a vector's components.
        /// </summary>
        /// <param name="v1">The vector whose scalar components to multiply by a power.</param>
        /// <param name="power">The power by which to multiply the components.</param>
        /// <returns>The multiplied Vector3.</returns>
        [Description("The individual multiplication to a power of a vector's components.")]
        public static Vector3 PowComponents(Vector3 v1, double power)
        {
            return
            (
                new Vector3
                    (
                        Math.Pow(v1.X, power),
                        Math.Pow(v1.Y, power),
                        Math.Pow(v1.Z, power)
                    )
            );
        }

        /// <summary>
        /// The individual multiplication to a power of this vector's components.
        /// </summary>
        /// <param name="power">The power by which to multiply the components.</param>
        /// <returns>The multiplied Vector3.</returns>
        /// <implementation>
        /// <see cref="PowComponents(Vector3, Double)"/>
        /// The Components.PowComponents(Vector3, double) function has been used to prevent code duplication
        /// </implementation>
        [Description("The individual multiplication to a power of this vector's components.")]
        public void PowComponents(double power)
        {
            this = PowComponents(this, power);
        }

        /// <summary>
        /// The individual square root of a vector's components.
        /// </summary>
        /// <param name="v1">The vector whose scalar components to square root</param>
        /// <returns>The rooted Vector3.</returns>
        [Description("The individual square root of a vector's components.")]
        public static Vector3 SqrtComponents(Vector3 v1)
        {
            return
                (
                new Vector3
                    (
                        Math.Sqrt(v1.X),
                        Math.Sqrt(v1.Y),
                        Math.Sqrt(v1.Z)
                    )
                );
        }

        /// <summary>
        /// The individual square root of this vector's components.
        /// </summary>
        /// <returns>The rooted Vector3.</returns>
        /// <implementation>
        /// <see cref="SqrtComponents(Vector3)"/>
        /// The Components.SqrtComponents(Vector3) function has been used to prevent code duplication
        /// </implementation>
        [Description("The individual square root of this vector's components.")]
        public void SqrtComponents()
        {
            this = SqrtComponents(this);
        }

        /// <summary>
        /// The vector's components squared.
        /// </summary>
        /// <param name="v1">The vector whose scalar components are to square.</param>
        /// <returns>The squared Vector3.</returns>
        [Description("The vector's components squared.")]
        public static Vector3 SqrComponents(Vector3 v1)
        {
            return
                (
                new Vector3
                    (
                        v1.X * v1.X,
                        v1.Y * v1.Y,
                        v1.Z * v1.Z
                    )
                );
        }

        /// <summary>
        /// This vector's components squared.
        /// </summary>
        /// <returns>The squared Vector3.</returns>
        /// <implementation>
        /// <see cref="SqrtComponents(Vector3)"/>
        /// The Components.SqrComponents(Vector3) function has been used to prevent code duplication
        /// </implementation>
        [Description("This vector's components squared.")]
        public void SqrComponents()
        {
            this = SqrtComponents(this);
        }

        #endregion

        #region Standard Functions
        /// <summary>
        /// Textual description of the vector.
        /// </summary>
        /// <Implementation>
        /// Uses ToString(string, IFormatProvider) to avoid code duplication
        /// </Implementation>
        /// <returns>Text (String) representing the vector.</returns>
        [Description("Textual description of the vector.")]
        public override string ToString()
        {
            return X.ToString("N2") + ", " + Y.ToString("N2") + ", " + Z.ToString("N2");
        }

        /// <summary>
        /// Textual description of the vector, with component labels.
        /// </summary>
        /// <returns>Text (String) representing the vector.</returns>
        [Description("Textual description of the vector, with component labels.")]
        public string ToLabelledString()
        {
            return "X: " + X.ToString("N2") + ", Y: " + Y.ToString("N2") + ", Z: " + Z.ToString("N2");
        }

        /// <summary>
        /// Attempts to parse vector data from a string.
        /// </summary>
        /// <param name="text">The text to parse.</param>
        /// <param name="value">The resulting Vector3 value.</param>
        /// <returns>True if parsing succeeded, false if not.</returns>
        [Description("Attempts to parse vector data from a string.")]
        public static bool TryParse(string text, out Vector3 value)
        {
            value = new Vector3();
            double x, y, z;

            string[] token = text.Trim(new char[] {' ', '{', '}'}).Replace("X: ", "").Replace("Y: ", "").Replace("Z: ", "").Split(',');
            if (token.Length != 3) return false;

            if (!double.TryParse(token[0], out x)) return false;
            if (!double.TryParse(token[1], out y)) return false;
            if (!double.TryParse(token[2], out z)) return false;

            value = new Vector3(x, y, z);
            return true;
        }

        /// <summary>
        /// Parses vector data from a string.  Throws a FormatException on failure.
        /// </summary>
        /// <param name="text">The text to parse.</param>
        /// <returns>The resulting Vector3 value.</returns>
        [Description("Parses vector data from a string.  Throws a FormatException on failure.")]
        public static Vector3 Parse(string text)
        {
            Vector3 retval = new Vector3();

            try
            {
                string[] token = text.Trim(new char[] { ' ', '{', '}' }).Replace("X: ", "").Replace("Y: ", "").Replace("Z: ", "").Split(new char[] { ',', ' ' });
                if (token.Length != 3) throw new Exception();
                retval.X = double.Parse(token[0].Trim());
                retval.Y = double.Parse(token[1].Trim());
                retval.Z = double.Parse(token[2].Trim());
            }
            catch
            {
                throw new FormatException("Unable to parse this string as Vector3 data.");
            }

            return retval;
        }

        /// <summary>
        /// Textual description of the vector.
        /// </summary>
        /// <param name="format">Formatting string: 'x','y','z' or '' followed by standard numeric format string characters valid for a double precision floating point</param>
        /// <param name="formatProvider">The culture specific fromatting provider</param>
        /// <returns>Text (String) representing the vector</returns>
        [Description("Textual description of the vector.")]
        public string ToString(string format, IFormatProvider formatProvider)
        {
            // If no format is passed
            if (format == null || format == "") return String.Format("({0}, {1}, {2})", X, Y, Z);

            char firstChar = format[0];
            string remainder = null;

            if (format.Length > 1)
                remainder = format.Substring(1);

            switch (firstChar)
            {
                case 'x': return X.ToString(remainder, formatProvider);
                case 'y': return Y.ToString(remainder, formatProvider);
                case 'z': return Z.ToString(remainder, formatProvider);
                default:
                    return String.Format
                        (
                            "({0}, {1}, {2})",
                            X.ToString(format, formatProvider),
                            Y.ToString(format, formatProvider),
                            Z.ToString(format, formatProvider)
                        );
            }
        }

        /// <summary>
        /// Get the hashcode.
        /// </summary>
        /// <returns>Hashcode for the object instance</returns>
        /// <implementation>
        /// Required in order to implement comparator operations (i.e. ==, !=)
        /// </implementation>
        /// <Acknowledgement>This code is adapted from CSOpenGL - Lucas Viñas Livschitz </Acknowledgement>
        [Description("Gets the hashcode.")]
        public override int GetHashCode()
        {
            return
            (
                (int)((X + Y + Z) % Int32.MaxValue)
            );
        }

        /// <summary>
        /// Comparator
        /// </summary>
        /// <param name="other">The other object (which should be a vector) to compare to</param>
        /// <returns>Truth if two vectors are equal within a tolerence</returns>
        /// <implementation>
        /// Checks if the object argument is a Vector3 object 
        /// Uses the equality operator function to avoid code duplication
        /// Required in order to implement comparator operations (i.e. ==, !=)
        /// </implementation>
        [Description("Checks for equality to another object.")]
        public override bool Equals(object other)
        {
            // Check object other is a Vector3 object
            if (other is Vector3)
            {
                // Convert object to Vector3
                Vector3 otherVector = (Vector3)other;

                // Check for equality
                return otherVector == this;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Comparator
        /// </summary>
        /// <param name="other">The other Vector3 to compare to</param>
        /// <returns>Truth if two vectors are equal within a tolerence</returns>
        /// <implementation>
        /// Uses the equality operator function to avoid code duplication
        /// </implementation>
        [Description("Checks for equality to another object.")]
        public bool Equals(Vector3 other)
        {
            return other == this;
        }

        /// <summary>
        /// Compares the magnitude of this instance against the magnitude of the supplied vector.
        /// </summary>
        /// <param name="other">The vector to compare this instance with</param>
        /// <returns>
        /// -1: The magnitude of this instance is less than the others magnitude
        /// 0: The magnitude of this instance equals the magnitude of the other
        /// 1: The magnitude of this instance is greater than the magnitude of the other
        /// </returns>
        /// <implementation>
        /// Implemented to fulfil the IComparable interface
        /// </implementation>
        /// <Acknowledgement>This code is adapted from Exocortex - Ben Houston </Acknowledgement>
        [Description("Compares the magnitude of this instance against the magnitude of the supplied vector.")]
        public int CompareTo(Vector3 other)
        {
            if (this < other)
            {
                return -1;
            }
            else if (this > other)
            {
                return 1;
            }
            return 0;
        }

        /// <summary>
        /// Compares the magnitude of this instance against the magnitude of the supplied vector.
        /// </summary>
        /// <param name="other">The vector to compare this instance with</param>
        /// <returns>
        /// -1: The magnitude of this instance is less than the others magnitude
        /// 0: The magnitude of this instance equals the magnitude of the other
        /// 1: The magnitude of this instance is greater than the magnitude of the other
        /// </returns>
        /// <implementation>
        /// Implemented to fulfil the IComparable interface
        /// </implementation>
        /// <exception cref="ArgumentException">
        /// Throws an exception if the type of object to be compared is not known to this class
        /// </exception>
        /// <Acknowledgement>This code is adapted from Exocortex - Ben Houston </Acknowledgement>
        [Description("Compares the magnitude of this instance against the magnitude of the supplied vector.")]
        public int CompareTo(object other)
        {
            if (other is Vector3)
            {
                return CompareTo((Vector3)other);
            }
            else
            {
                // Error condition: other is not a Vector3 object
                throw new ArgumentException
                (
                    // Error message includes information about the actual type of the argument
                    NON_VECTOR_COMPARISON + "\n" + ARGUMENT_TYPE + other.GetType().ToString(),
                    "other"
                );
            }
        }

        #endregion

        #region Decisions
        /// <summary>
        /// Checks if a vector a unit vector.
        /// Checks if the Vector3 has been normalized.
        /// Checks if a vector has a magnitude of 1.
        /// </summary>
        /// <param name="v1">
        /// The vector to be checked for Normalization.
        /// </param>
        /// <returns>True if the vector is a unit vector.</returns>
        /// <implementation>
        /// <see cref="Magnitude"/>	
        /// Uses the Magnitude property in the check to avoid code duplication
        /// Within a tolerence
        /// </implementation>
        [Description("Checks if a vector is a unit vector (ie. with magnitude of 1).")]
        public static bool IsUnitVector(Vector3 v1)
        {
            return Math.Abs(v1.Magnitude - 1) <= EqualityTolerence;
        }

        /// <summary>
        /// Checks if this vector is a unit vector.
        /// Checks if the Vector3 has been normalized.
        /// Checks if the vector has a magnitude of 1.
        /// </summary>
        /// <returns>True if this vector is a unit vector.</returns>
        /// <implementation>
        /// <see cref="IsUnitVector(Vector3)"/>	
        /// Uses the isUnitVector(Vector3) property in the check to avoid code duplication
        /// Within a tolerence
        /// </implementation>
        [Description("Checks if this vector is a unit vector (ie. with magnitude of 1).")]
        public bool IsUnitVector()
        {
            return IsUnitVector(this);
        }

        /// <summary>
        /// Checks if a face normal vector represents back face.
        /// Checks if a face is visible, given the line of sight.
        /// </summary>
        /// <param name="normal">
        /// The vector representing the face normal Vector3.
        /// </param>
        /// <param name="lineOfSight">
        /// The unit vector representing the direction of sight from a virtual camera.
        /// </param>
        /// <returns>True if the vector (as a normal) represents a back face.</returns>
        /// <implementation>
        /// Uses the DotProduct function in the check to avoid code duplication.
        /// </implementation>
        [Description("Checks if a face normal vector represents a back face, given a line of sight.")]
        public static bool IsBackFace(Vector3 normal, Vector3 lineOfSight)
        {
            return normal.DotProduct(lineOfSight) < 0;
        }

        /// <summary>
        /// Checks if this face normal vector represents back face.
        /// Checks if this face is visible, given the line of sight.
        /// </summary>
        /// <param name="lineOfSight">
        /// The unit vector representing the direction of sight from a virtual camera.
        /// </param>
        /// <returns>Truth if the vector (as a normal) represents a back face.</returns>
        /// <implementation>
        /// <see cref="Vector3.IsBackFace(Vector3, Vector3)"/> 
        /// Uses the isBackFace(Vector3, Vector3) function in the check to avoid code duplication.
        /// </implementation>
        [Description("Checks if this face normal vector represents a back face, given a line of sight.")]
        public bool IsBackFace(Vector3 lineOfSight)
        {
            return IsBackFace(this, lineOfSight);
        }

        /// <summary>
        /// Checks if two vectors are perpendicular.
        /// Checks if two Vectors are orthogonal.
        /// Checks if one Vector3 is the normal of the other.
        /// </summary>
        /// <param name="v1">
        /// The vector to be checked for orthogonality.
        /// </param>
        /// <param name="v2">
        /// The vector to be checked for orthogonality to.
        /// </param>
        /// <returns>Truth if the two Vectors are perpendicular.</returns>
        /// <implementation>
        /// Uses the DotProduct function in the check to avoid code duplication.
        /// </implementation>
        [Description("Checks if two vectors are perpendicular.")]
        public static bool IsPerpendicular(Vector3 v1, Vector3 v2)
        {
            return v1.DotProduct(v2) == 0;
        }

        /// <summary>
        /// Checks if this vector is perpendicular to another.
        /// Checks if two Vectors are orthogonal.
        /// Checks if one Vector3 is the Normal of the other.
        /// </summary>
        /// <param name="other">
        /// The vector to be checked for orthogonality.
        /// </param>
        /// <returns>Truth if the two Vectors are perpendicular.</returns>
        /// <implementation>
        /// Uses the isPerpendicualr(Vector3, Vector3) function in the check to avoid code duplication.
        /// </implementation>
        [Description("Checks if this vector is perpendicular to another.")]
        public bool IsPerpendicular(Vector3 other)
        {
            return IsPerpendicular(this, other);
        }

        #endregion

        #region ICloneable Members
        public object Clone()
        {
            return new Vector3(this);
        }
        #endregion
    }
}
