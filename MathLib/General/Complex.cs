using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace MathLib
{
    /// <summary>
    /// Structure representing a complex number
    /// </summary>
    [Serializable]
    public struct Complex
    {
        private double _real;
        private double _imag;

        ///<summary>
        /// Initialise a new instance of <see cref="Complex"/> with a 
        /// specified real and imaginary component.
        ///</summary>
        ///<param name="re">Real part of complex number.</param>
        ///<param name="im">Imaginary part of complex number.</param>
        public Complex(double re, double im)
        {
            _real = re;
            _imag = im;
        }


        ///<summary>
        /// Initialise a new instance of <see cref="Complex"/> by copying an
        /// existing complex number.
        ///</summary>
        ///<param name="c">The <see cref="Complex"/> number to copy.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "c")]
        public Complex(Complex c) : this(c._real, c._imag) { }

        ///<summary>
        /// Return the modulus of the <see cref="Complex"/> number
        ///</summary>
        public double Norm
        {
            get
            {
                return Math.Sqrt(_real * _real + _imag * _imag);
            }
        }

        ///<summary>
        /// Returns the real component of the <see cref="Complex"/> number.
        ///</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Re")]
        public double Re
        {
            get { return _real; }
            set { _real = value; }
        }

        ///<summary>
        /// Returns the imaginary component of the <see cref="Complex"/>
        /// number.
        ///</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Im")]
        public double Im
        {
            get { return _imag; }
            set { _imag = value; }
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <returns>
        /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        /// <param name="obj">Another object to compare to. </param><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            if (GetType() != obj.GetType()) return false;

            Complex rhs = (Complex)obj;

            if (rhs._real != _real || rhs._imag != _imag)
                return false;

            return true;            
        }
               
        ///<summary>
        /// Returns a <see cref="Complex"/> number as calculated from its
        /// Polar coordinates.
        ///</summary>
        ///<param name="r">Distance component of <see cref="Complex"/> number.</param>
        ///<param name="theta">Angular component of <see cref="Complex"/> number.</param>
        ///<returns>A new <see cref="Complex"/> number with the specified
        /// magnitude and angle.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "r")]
        public static Complex FromPolar(double r, double theta)
        {
            return new Complex(r * Math.Cos(theta), r * Math.Sin(theta));
        }

        /// <summary>
        /// Generate string representation of <see cref="Complex"/> number.
        /// </summary>
        /// <returns>String representation of <see cref="Complex"/> number.</returns>
        public override string ToString()
        {
            return String.Format(CultureInfo.CurrentCulture, "{0,10:0.0000} + {1,10:0.0000i}", _real, _imag);
        }


        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return _real.GetHashCode() ^ _imag.GetHashCode();
        }

        //Operator declarations
        ///<summary>
        /// Negate <see cref="Complex"/> number.
        ///</summary>
        ///<param name="c"><see cref="Complex"/> number to negate.</param>
        ///<returns>A new <see cref="Complex"/> number equal to 
        /// -<paramref name="c"/>.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "c")]
        public static Complex Negate(Complex c)
        {
            return -c;
        }

        //Operator declarations
        ///<summary>
        /// Negate <see cref="Complex"/> number.
        ///</summary>
        ///<param name="c"><see cref="Complex"/> number to negate.</param>
        ///<returns>A new <see cref="Complex"/> number equal to 
        /// -<paramref name="c"/>.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "c")]
        public static Complex operator -(Complex c)
        {
            return new Complex(-c._real, -c._imag);           
        }

        ///<summary>
        /// Subtract one <see cref="Complex"/> number from another.
        ///</summary>
        ///<param name="lhs">Left hand side of the subtraction.</param>
        ///<param name="rhs">Right hand side of the subtraction.</param>
        ///<returns>Result of the subtraction</returns>
        public static Complex Subtract(Complex lhs, Complex rhs)
        {
            return lhs - rhs;
        }

        ///<summary>
        /// Subtract one <see cref="Complex"/> number from another.
        ///</summary>
        ///<param name="lhs">Left hand side of the subtraction.</param>
        ///<param name="rhs">Right hand side of the subtraction.</param>
        ///<returns>Result of the subtraction</returns>
        public static Complex operator -(Complex lhs, Complex rhs)
        {   
            return new Complex(lhs._real - rhs._real, lhs._imag - rhs._imag);            
        }

        ///<summary>
        /// Add one <see cref="Complex"/> number to another.
        ///</summary>
        ///<param name="lhs">Left hand side of the addition.</param>
        ///<param name="rhs">Right hand side of the addition.</param>
        ///<returns>Result of the addition.</returns>
        public static Complex Add(Complex lhs, Complex rhs)
        {
            return lhs + rhs;
        }

        ///<summary>
        /// Add one <see cref="Complex"/> number to another.
        ///</summary>
        ///<param name="lhs">Left hand side of the addition.</param>
        ///<param name="rhs">Right hand side of the addition.</param>
        ///<returns>Result of the addition.</returns>
        public static Complex operator +(Complex lhs, Complex rhs)
        {
            return new Complex(lhs._real + rhs._real, lhs._imag + rhs._imag);
        }

        ///<summary>
        /// Multiple a <see cref="Complex"/> number by a double 
        /// precision number.
        ///</summary>
        ///<param name="lhs">Double precision number on the left hand
        /// side of the multiplication.</param>
        ///<param name="rhs"><see cref="Complex"/> number on the right hand
        /// side of the multiplication.</param>
        ///<returns>Result of the multiplication.</returns>
        public static Complex Multiply(double lhs, Complex rhs)
        {
            return lhs*rhs;
        }

        ///<summary>
        /// Multiple a <see cref="Complex"/> number by a double 
        /// precision number.
        ///</summary>
        ///<param name="lhs">Double precision number on the left hand
        /// side of the multiplication.</param>
        ///<param name="rhs"><see cref="Complex"/> number on the right hand
        /// side of the multiplication.</param>
        ///<returns>Result of the multiplication.</returns>
        public static Complex operator *(double lhs, Complex rhs)
        {
            return new Complex(rhs._real * lhs, rhs._imag * lhs);            
        }

        ///<summary>
        /// Multiple a <see cref="Complex"/> number by a double 
        /// precision number.
        ///</summary>
        ///<param name="lhs"><see cref="Complex"/> number number on the left
        /// hand side of the multiplication.</param>
        ///<param name="rhs">Double precision number on the right hand
        /// side of the multiplication.</param>
        ///<returns>Result of the multiplication.</returns>
        public static Complex Multiply(Complex lhs, double rhs)
        {
            return lhs*rhs;
        }

        ///<summary>
        /// Multiple a <see cref="Complex"/> number by a double 
        /// precision number.
        ///</summary>
        ///<param name="lhs"><see cref="Complex"/> number number on the left
        /// hand side of the multiplication.</param>
        ///<param name="rhs">Double precision number on the right hand
        /// side of the multiplication.</param>
        ///<returns>Result of the multiplication.</returns>
        public static Complex operator *(Complex lhs, double rhs)
        {
            return new Complex(lhs._real * rhs, lhs._imag * rhs);
        }

        ///<summary>
        /// Multiply two <see cref="Complex"/> numbers.
        ///</summary>
        ///<param name="lhs">Left hand side of the multiplication.</param>
        ///<param name="rhs">Right hand side of the multiplication.</param>
        ///<returns>Result of the multiplication.</returns>
        public static Complex operator *(Complex lhs, Complex rhs)
        {
            return new Complex(lhs._real * rhs._real - lhs._imag * rhs._imag, 
                lhs._real * rhs._imag + lhs._imag * rhs._real);
        }

        ///<summary>
        /// Divide a <see cref="Complex"/> number by a double 
        /// precision number.
        ///</summary>
        ///<param name="lhs"><see cref="Complex"/> number on the left hand
        /// side of the division.</param>
        ///<param name="rhs">Double precision number on the right hand 
        /// side of the division.</param>
        ///<returns>Result of the division.</returns>
        public static Complex Divide(Complex lhs, double rhs)
        {
            return lhs/rhs;
        }

        ///<summary>
        /// Divide a <see cref="Complex"/> number by a double 
        /// precision number.
        ///</summary>
        ///<param name="lhs"><see cref="Complex"/> number on the left hand
        /// side of the division.</param>
        ///<param name="rhs">Double precision number on the right hand 
        /// side of the division.</param>
        ///<returns>Result of the division.</returns>
        public static Complex operator /(Complex lhs, double rhs)
        {
            return new Complex(lhs._real / rhs, lhs._imag / rhs);
        }

        ///<summary>
        /// Test two <see cref="Complex"/> numbers for equality.
        ///</summary>
        ///<param name="lhs">Left hand side of equality.</param>
        ///<param name="rhs">Right hand side of equality.</param>
        ///<returns><see langword="true"/> if two <see cref="Complex"/>
        /// numbers are equal, <see langword="false"/> otherwise.</returns>
        public static bool operator ==(Complex lhs, Complex rhs)
        {
            return lhs.Equals(rhs);
        }

        ///<summary>
        /// Test two <see cref="Complex"/> numbers for inequality.
        ///</summary>
        ///<param name="lhs">Left hand side of inequality.</param>
        ///<param name="rhs">Right hand side of inequality.</param>
        ///<returns><see langword="true"/> if two <see cref="Complex"/>
        /// numbers are not equal, <see langword="false"/> otherwise.</returns>
        public static bool operator !=(Complex lhs, Complex rhs)
        {
            return !lhs.Equals(rhs);
        }

        ///<summary>
        /// Compare two instance of the <see cref="Complex"/> type.
        ///</summary>
        ///<param name="lhs">Left hand side of comparison.</param>
        ///<param name="rhs">Right hand side of comparison</param>
        ///<returns>0 if <paramref name="lhs"/> = <paramref name="rhs"/>,
        /// -1 if <paramref name="lhs"/> &lt; <paramref name="rhs"/>, 1 if
        /// <paramref name="lhs"/> &gt; <paramref name="rhs"/>.</returns>
        public static int Compare(Complex lhs, Complex rhs)
        {
            if (lhs == rhs)
                return 0;
            if (lhs < rhs)
                return -1;

            return 1;
        }

        ///<summary>
        /// Tests whether one <see cref="Complex"/> number is less than 
        /// another.
        ///</summary>
        ///<param name="lhs">Left hand side of less than operator.</param>
        ///<param name="rhs">Right hand side of less than operator.</param>
        ///<returns><see langword="true"/> if <paramref name="lhs"/> is
        /// less than <paramref name="rhs"/>, <see langword="false"/> otherwise.</returns>
        public static bool operator <(Complex lhs, Complex rhs)
        {
            if (lhs.Norm < rhs.Norm)
                return true;
            return false;
        }

        ///<summary>
        /// Tests whether one <see cref="Complex"/> number is greater than 
        /// another.
        ///</summary>
        ///<param name="lhs">Left hand side of greater than operator.</param>
        ///<param name="rhs">Right hand side of greater than operator.</param>
        ///<returns><see langword="true"/> if <paramref name="lhs"/> is
        /// greater than <paramref name="rhs"/>, <see langword="false"/> otherwise.</returns>
        public static bool operator >(Complex lhs, Complex rhs)
        {
            if (lhs.Norm < rhs.Norm)
                return true;
            return false;
        }
    }
}
