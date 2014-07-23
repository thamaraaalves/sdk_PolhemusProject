using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Globalization;

namespace ListenPortG4
{
    #region Type Converter
    /// <summary>
    /// Handles converting an Angle to a string, and back again.
    /// </summary>
    [Description("Handles converting an Angle to a string, and back again.")]
    public class AngleTypeConverter : StringConverter
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
                string[] tokens = value.ToString().Split(' ');

                try
                {
                    return new Angle(double.Parse(tokens[0]), (AngleUnit)Enum.Parse(typeof(AngleUnit), tokens[1], true));
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
            return value is Angle;
        }
    }
    #endregion

    [Description("Enumeration representing a unit of measure for an angle.")]
    public enum AngleUnit
    {
        Radians,
        Degrees
    }

    /// <summary>
    /// Class to represent an angle and easily convert between units.
    /// </summary>
    [Serializable, TypeConverter("AngleTypeConverter"), Description("Class to represent an angle and easily convert between units.")]
    public class Angle : IComparable
    {
        private double nativeangle = 0;
        private AngleUnit nativeunit = AngleUnit.Radians;
        internal bool undefined = false;

        public Angle()
        {
        }

        public Angle(double angle, AngleUnit unit)
        {
            Set(angle, unit);
        }

        #region Static Properties
        /// <summary>
        /// Property that represents and undefined angle.
        /// </summary>
        [Description("Property that represents and undefined angle.")]
        public static Angle Undefined
        {
            get
            {
                Angle retval = new Angle();
                retval.undefined = true;
                return retval;
            }
        }

        /// <summary>
        /// Shortcut to obtain an angle of 0 degrees.
        /// </summary>
        [Description("Shortcut to obtain an angle of 0 degrees.")]
        public static Angle Angle0Degrees
        {
            get
            {
                return new Angle();
            }
        }

        /// <summary>
        /// Shortcut to obtain an angle of 90 degrees.
        /// </summary>
        [Description("Shortcut to obtain an angle of 90 degrees.")]
        public static Angle Angle90Degrees
        {
            get
            {
                return new Angle(90, AngleUnit.Degrees);
            }
        }

        /// <summary>
        /// Shortcut to obtain an angle of 180 degrees.
        /// </summary>
        [Description("Shortcut to obtain an angle of 180 degrees.")]
        public static Angle Angle180Degrees
        {
            get
            {
                return new Angle(180, AngleUnit.Degrees);
            }
        }

        /// <summary>
        /// Shortcut to obtain an angle of 270 degrees.
        /// </summary>
        [Description("Shortcut to obtain an angle of 270 degrees.")]
        public static Angle Angle270Degrees
        {
            get
            {
                return new Angle(270, AngleUnit.Degrees);
            }
        }

        /// <summary>
        /// Shortcut to obtain an angle of 360 degrees.
        /// </summary>
        [Description("Shortcut to obtain an angle of 360 degrees.")]
        public static Angle Angle360Degrees
        {
            get
            {
                return new Angle(360, AngleUnit.Degrees);
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// This angle in radians.
        /// </summary>
        [Description("This angle in radians.")]
        public double Radians
        {
            get
            {
                if (undefined) throw new Exception("The angle value is undefined, and does not have a radian measure.");
                return GetValue(AngleUnit.Radians);
            }
            set
            {
                Set(value, AngleUnit.Radians);
            }
        }

        /// <summary>
        /// This angle in degrees.
        /// </summary>
        [Description("This angle in degrees.")]
        public double Degrees
        {
            get
            {
                if (undefined) throw new Exception("The angle value is undefined, and does not have a degree measure.");
                return GetValue(AngleUnit.Degrees);
            }
            set
            {
                Set(value, AngleUnit.Degrees);
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Sets the angle value.
        /// </summary>
        /// <param name="angle">The angle value.</param>
        /// <param name="unit">The unit.</param>
        [Description("Sets the angle value.")]
        public void Set(double angle, AngleUnit unit)
        {
            nativeangle = angle;
            nativeunit = unit;
            undefined = false;
        }

        /// <summary>
        /// Sets the angle value from opposite and adjacent lengths.
        /// </summary>
        /// <param name="opp">The opposite length.</param>
        /// <param name="adj">The adjacent length.</param>
        [Description("Sets the angle value from opposite and adjacent lengths.")]
        public void SetOppAdj(double opp, double adj)
        {
            nativeunit = AngleUnit.Radians;
            nativeangle = Math.Atan(opp / adj);
            undefined = false;
        }

        /// <summary>
        /// Sets the angle value from opposite and hypoteneuse lengths.
        /// </summary>
        /// <param name="opp">The opposite length.</param>
        /// <param name="hyp">The hypoteneuse length.</param>
        [Description("Sets the angle value from opposite and hypoteneuse lengths.")]
        public void SetOppHyp(double opp, double hyp)
        {
            nativeunit = AngleUnit.Radians;
            nativeangle = Math.Asin(opp / hyp);
            undefined = false;
        }

        /// <summary>
        /// Sets the angle value from adjacent and hypoteneuse lengths.
        /// </summary>
        /// <param name="adj">The adjacent length.</param>
        /// <param name="hyp">The hypoteneuse length.</param>
        [Description("Sets the angle value from adjacent and hypoteneuse lengths.")]
        public void SetAdjHyp(double adj, double hyp)
        {
            nativeunit = AngleUnit.Radians;
            nativeangle = Math.Acos(adj / hyp);
            undefined = false;
        }

        /// <summary>
        /// Gets the value in the specified units.
        /// </summary>
        /// <param name="unit">The desired units.</param>
        /// <returns>The value in desired units.</returns>
        [Description("Gets the value in the specified units.")]
        public double GetValue(AngleUnit unit)
        {
            if (nativeunit == unit) return nativeangle;

            // Conversion required
            switch (nativeunit)
            {
                case AngleUnit.Degrees:
                    switch (unit)
                    {
                        case AngleUnit.Radians:
                            return nativeangle * Math.PI / 180.0;
                    }
                    break;

                case AngleUnit.Radians:
                    switch (unit)
                    {
                        case AngleUnit.Degrees:
                            return nativeangle * 180 / Math.PI;
                    }
                    break;
            }

            return nativeangle;
        }

        /// <summary>
        /// Gets the absolute value of the angle.
        /// </summary>
        /// <returns>The value.</returns>
        [Description("Gets the absolute value of the angle.")]
        public Angle Abs()
        {
            if (undefined) return Angle.Undefined;
            return new Angle(Math.Abs(nativeangle), nativeunit);
        }

        /// <summary>
        /// Gets the angle value as a string.
        /// </summary>
        /// <returns>A string.</returns>
        [Description("Gets the angle value as a string.")]
        public override string ToString()
        {
            if (undefined) return "Undefined";
            return GetValue(AngleUnit.Degrees).ToString("N2") + " Degrees";
        }

        /// <summary>
        /// Gets the angle as a string, in the given units.
        /// </summary>
        /// <param name="unit">The desired units.</param>
        /// <returns>A string.</returns>
        [Description("Gets the angle as a string, in the given units.")]
        public string ToString(AngleUnit unit)
        {
            if (undefined) return "Undefined";
            return GetValue(unit).ToString() + " " + unit.ToString();
        }
        #endregion

        #region Mathematical Operators
        /// <summary>
        /// Angle addition.
        /// </summary>
        /// <param name="a1">One angle.</param>
        /// <param name="a2">Another angle.</param>
        /// <returns>The sum.</returns>
        public static Angle operator +(Angle a1, Angle a2)
        {
            if (a1 == Angle.Undefined || a2 == Angle.Undefined) return Angle.Undefined;
            return new Angle(a1.GetValue(AngleUnit.Radians) + a2.GetValue(AngleUnit.Radians), AngleUnit.Radians);
        }

        /// <summary>
        /// Angle multiplication.
        /// </summary>
        /// <param name="a1">One angle.</param>
        /// <param name="a2">Another angle.</param>
        /// <returns>The product.</returns>
        public static Angle operator *(Angle a1, Angle a2)
        {
            if (a1 == Angle.Undefined || a2 == Angle.Undefined) return Angle.Undefined;
            return new Angle(a1.GetValue(AngleUnit.Radians) * a2.GetValue(AngleUnit.Radians), AngleUnit.Radians);
        }

        /// <summary>
        /// Angle subtraction.
        /// </summary>
        /// <param name="a1">One angle.</param>
        /// <param name="a2">Another angle.</param>
        /// <returns>The difference.</returns>
        public static Angle operator -(Angle a1, Angle a2)
        {
            if (a1 == Angle.Undefined || a2 == Angle.Undefined) return Angle.Undefined;
            return new Angle(a1.GetValue(AngleUnit.Radians) - a2.GetValue(AngleUnit.Radians), AngleUnit.Radians);
        }

        /// <summary>
        /// Angle division.
        /// </summary>
        /// <param name="a1">One angle.</param>
        /// <param name="a2">Another angle.</param>
        /// <returns>The quotient.</returns>
        public static Angle operator /(Angle a1, Angle a2)
        {
            if (a1 == Angle.Undefined || a2 == Angle.Undefined || a2.Radians == 0) return Angle.Undefined;
            return new Angle(a1.GetValue(AngleUnit.Radians) / a2.GetValue(AngleUnit.Radians), AngleUnit.Radians);
        }

        /// <summary>
        /// Angle to scalar addition.
        /// </summary>
        /// <param name="a1">An angle.</param>
        /// <param name="val">A scalar.</param>
        /// <returns>The sum.</returns>
        public static Angle operator +(Angle a1, double val)
        {
            if (a1 == Angle.Undefined) return Angle.Undefined;
            return new Angle(a1.GetValue(AngleUnit.Radians) + val, AngleUnit.Radians);
        }

        /// <summary>
        /// Angle to scalar multiplication.
        /// </summary>
        /// <param name="a1">An angle.</param>
        /// <param name="val">A scalar.</param>
        /// <returns>The product.</returns>
        public static Angle operator *(Angle a1, double val)
        {
            if (a1 == Angle.Undefined) return Angle.Undefined;
            return new Angle(a1.GetValue(AngleUnit.Radians) * val, AngleUnit.Radians);
        }

        /// <summary>
        /// Angle to scalar subtraction.
        /// </summary>
        /// <param name="a1">An angle.</param>
        /// <param name="val">A scalar.</param>
        /// <returns>The difference.</returns>
        public static Angle operator -(Angle a1, double val)
        {
            if (a1 == Angle.Undefined) return Angle.Undefined;
            return new Angle(a1.GetValue(AngleUnit.Radians) - val, AngleUnit.Radians);
        }

        /// <summary>
        /// Angle to scalar division.
        /// </summary>
        /// <param name="a1">An angle.</param>
        /// <param name="val">A scalar.</param>
        /// <returns>The quotient.</returns>
        public static Angle operator /(Angle a1, double val)
        {
            if (a1 == Angle.Undefined || val == 0) return Angle.Undefined;
            return new Angle(a1.GetValue(AngleUnit.Radians) / val, AngleUnit.Radians);
        }

        /// <summary>
        /// Scalar to angle addition.
        /// </summary>
        /// <param name="val">A scalar.</param>
        /// <param name="a1">An angle.</param>
        /// <returns>The sum.</returns>
        public static Angle operator +(double val, Angle a1)
        {
            if (a1 == Angle.Undefined) return Angle.Undefined;
            return new Angle(val + a1.Radians, AngleUnit.Radians);
        }

        /// <summary>
        /// Scalar to angle multiplication.
        /// </summary>
        /// <param name="val">A scalar.</param>
        /// <param name="a1">An angle.</param>
        /// <returns>The product.</returns>
        public static Angle operator *(double val, Angle a1)
        {
            if (a1 == Angle.Undefined) return Angle.Undefined;
            return new Angle(val * a1.Radians, AngleUnit.Radians);
        }

        /// <summary>
        /// Scalar to angle subtraction.
        /// </summary>
        /// <param name="val">A scalar.</param>
        /// <param name="a1">An angle.</param>
        /// <returns>The difference.</returns>
        public static Angle operator -(double val, Angle a1)
        {
            if (a1 == Angle.Undefined) return Angle.Undefined;
            return new Angle(val - a1.Radians, AngleUnit.Radians);
        }

        /// <summary>
        /// Scalar to angle division.
        /// </summary>
        /// <param name="val">A scalar.</param>
        /// <param name="a1">An angle.</param>
        /// <returns>The quotient.</returns>
        public static Angle operator /(double val, Angle a1)
        {
            if (a1 == Angle.Undefined || a1.Radians == 0) return Angle.Undefined;
            return new Angle(val / a1.Radians, AngleUnit.Radians);
        }
        #endregion

        #region Comparison Operators
        /// <summary>
        /// Angle to angle comparison.
        /// </summary>
        /// <param name="a1">One angle.</param>
        /// <param name="a2">Another angle.</param>
        /// <returns>The result.</returns>
        public static bool operator <(Angle a1, Angle a2)
        {
            return a1.CompareTo(a2) < 0;
        }

        /// <summary>
        /// Angle to angle comparison.
        /// </summary>
        /// <param name="a1">One angle.</param>
        /// <param name="a2">Another angle.</param>
        /// <returns>The result.</returns>
        public static bool operator >(Angle a1, Angle a2)
        {
            return a1.CompareTo(a2) > 0;
        }

        /// <summary>
        /// Angle to angle comparison.
        /// </summary>
        /// <param name="a1">One angle.</param>
        /// <param name="a2">Another angle.</param>
        /// <returns>The result.</returns>
        public static bool operator <=(Angle a1, Angle a2)
        {
            return a1.CompareTo(a2) <= 0;
        }

        /// <summary>
        /// Angle to angle comparison.
        /// </summary>
        /// <param name="a1">One angle.</param>
        /// <param name="a2">Another angle.</param>
        /// <returns>The result.</returns>
        public static bool operator >=(Angle a1, Angle a2)
        {
            return a1.CompareTo(a2) >= 0;
        }

        /// <summary>
        /// Angle to angle comparison.
        /// </summary>
        /// <param name="a1">One angle.</param>
        /// <param name="a2">Another angle.</param>
        /// <returns>The result.</returns>
        public static bool operator ==(Angle a1, Angle a2)
        {
            return a1.CompareTo(a2) == 0;
        }

        /// <summary>
        /// Angle to angle comparison.
        /// </summary>
        /// <param name="a1">One angle.</param>
        /// <param name="a2">Another angle.</param>
        /// <returns>The result.</returns>
        public static bool operator !=(Angle a1, Angle a2)
        {
            return a1.CompareTo(a2) != 0;
        }
        #endregion

        #region Equality Overloads
        /// <summary>
        /// Equality tester.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>Whether equal.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Angle)
            {
                Angle a2 = (Angle)obj;

                if (this.undefined && a2.undefined) return true;
                if (this.undefined || a2.undefined) return false;
                return this.Radians == a2.Radians;
            }
            return false;
        }

        /// <summary>
        /// Gets the hash code.
        /// </summary>
        /// <returns>An int.</returns>
        public override int GetHashCode()
        {
            if (this.undefined) return double.NaN.GetHashCode();
            else return this.Radians.GetHashCode();
        }
        #endregion

        #region IComparable Members
        /// <summary>
        /// Comparison method.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>Comparison result.</returns>
        public int CompareTo(object obj)
        {
            if (obj is Angle)
            {
                return nativeangle.CompareTo(((Angle)obj).GetValue(nativeunit));
            }
            else if (obj is double)
            {
                return nativeangle.CompareTo((double)obj);
            }
            else throw new Exception("Cannot compare angle to type " + obj.GetType().Name + "!");
        }
        #endregion
    }
}
