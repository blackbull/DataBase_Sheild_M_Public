using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ResultOptionsClassLibrary
{

    [Serializable]
    /// <summary>
    /// Реализует основы работы с комплексными числами
    /// </summary>
    public class ComplexClass : Object, IDisposable 
    {
        /// <summary>
        /// Числа менее этого будут обращены в нуль >:-|
        /// </summary>
        static public double precision = 1e-12;
        /// <summary>
        /// Отвечает за то каким образом будет отображаться мнимая единица
        /// </summary>
        public enum ComplexSymbolEnum { i,j};

        [NonSerialized]
        ComplexSymbolEnum _complexSymbol = ComplexSymbolEnum.i;

        public ComplexSymbolEnum ComplexSymbol
        {
            get { return _complexSymbol; }
            set { _complexSymbol = value; }
        }

        static public ComplexClass î
        {
            get { return new ComplexClass(0,1); }
            
        }

        /// <summary>
        /// Вывод комплексных чисел как строку
        /// </summary>
        /// <returns>возврат строки</returns>
        private String ComplexSymbolToString()
        {
            switch (this._complexSymbol)
            {
                case ComplexSymbolEnum.i:
                    {
                        return "i";
                        break;
                    }
                case ComplexSymbolEnum.j:
                    {
                        return "j";
                        break;
                    }
                default:
                    {
                        return "I";
                        break;
                    }
            }
        }
        Double _real = 0;
        /// <summary>
        /// Реальная часть комплексного числа
        /// </summary>
        public Double Real
        {
            get 
            {
                return _real; 
            }
            set 
            { 
                _real = value;
                if (Math.Abs(_real) < precision)
                    _real = 0;
            }
        }

        Double _imaginary = 0;
        /// <summary>
        /// Мнимая часть комплексного числа
        /// </summary>
        public Double Imaginary
        {
            get 
            {
                return _imaginary; 
            }
            set 
            { 
                _imaginary = value;
                if (Math.Abs(_imaginary) < precision)
                    _imaginary = 0;
            }
        }

        /// <summary>
        /// Амплитуда (модуль) мнимого числа
        /// </summary>
        public Double Amplitude
        {
            get 
            {
                if (_real.Equals(Double.NaN) || _imaginary.Equals(Double.NaN))
                {
                    return Double.NaN;
                }
                Double a = Math.Pow(_real, 2);
                Double b = Math.Pow(_imaginary, 2);
                return Math.Sqrt(a+b); 
            }
            set 
            {
                Double ampPrev=Amplitude;
                Double phaPrev = Phase;

                _real = value * Math.Cos(phaPrev);
                _imaginary = value * Math.Sin(phaPrev);
            }
        }

        /// <summary>
        /// Фаза (аргумент) мнимого числа в радианах
        /// </summary>
        public Double Phase
        {
            get
            {
                if (_real.Equals(Double.NaN) || _imaginary.Equals(Double.NaN))
                {
                    return Double.NaN;
                }
                if (_real == 0)
                {
                    return Math.Sign(_imaginary) * Math.PI / 2;
                }
                else
                {
                    return Math.Sign(_imaginary) * Math.PI / 2 * (1 - Math.Sign(_real)) + Math.Atan(_imaginary / _real);
                }
            }
            set 
            {
                Double amp = Amplitude;
                _real = amp * Math.Cos(value);
                _imaginary = amp * Math.Sin(value);
            }
        }

        public ComplexClass Conj
        {
            get
            {
                ComplexClass ret=new ComplexClass();
                ret._real = this._real;
                ret._imaginary = -this._imaginary;
                return ret;
            }
            
        }

        static public ComplexClass Exp(ComplexClass value)
        {
            ComplexClass ret = new ComplexClass();
            ret._real=Math.Exp(value._real)*Math.Cos(value._imaginary);
            ret._imaginary = Math.Exp(value._real) * Math.Sin(value._imaginary);
            return ret;
        }
        /// <summary>
        /// Создает объект комплексного числа нулевого значения
        /// </summary>
        public ComplexClass()
        {
        }

        /// <summary>
        /// Создает объект комплексного числа
        /// </summary>
        /// <param name="real">Реальная часть</param>
        /// <param name="imaginary">Мнимая часть</param>
        public ComplexClass(Double real, Double imaginary)
        {
            Real = real;
            Imaginary = imaginary;
        }

        /// <summary>
        /// Создает объект комплексного числа копированием
        /// </summary>
        /// <param name="val">копируемое число</param>
        public ComplexClass(ComplexClass val)
        {
            Real = val.Real;
            Imaginary = val.Imaginary;
        }

        /// <summary>
        /// Создает объект комплексного числа
        /// </summary>
        /// <param name="real">Реальная часть</param>
        /// <returns></returns>
        public static implicit operator ComplexClass(Double real)
        {
            return new ComplexClass(real, 0);
        }

        
        public static explicit operator Double[](ComplexClass val)
        {
            return new Double[] { val._real, val._imaginary };
        }

        #region Operators
        #region binary operator +
        static public ComplexClass operator +(ComplexClass v1, ComplexClass v2)
        {
            ComplexClass ret = new ComplexClass();
            ret._real = v1._real + v2._real;
            ret._imaginary = v1._imaginary + v2._imaginary;
            return ret;
        }
        static public ComplexClass operator +(ComplexClass v1, Double v2)
        {
            ComplexClass ret = new ComplexClass();
            ret._real = v1._real + v2;
            ret._imaginary = v1._imaginary;
            return ret;
        }
        static public ComplexClass operator +(Double v1, ComplexClass v2)
        {
            ComplexClass ret = new ComplexClass();
            ret._real = v1 + v2._real;
            ret._imaginary = v2._imaginary;
            return ret;
        }
        #endregion

        #region binary operator -
        static public ComplexClass operator -(ComplexClass v1, ComplexClass v2)
        {
            ComplexClass ret = new ComplexClass();
            ret._real = v1._real - v2._real;
            ret._imaginary = v1._imaginary - v2._imaginary;
            return ret;
        }
        static public ComplexClass operator -(ComplexClass v1, Double v2)
        {
            ComplexClass ret = new ComplexClass();
            ret._real = v1._real - v2;
            ret._imaginary = v1._imaginary;
            return ret;
        }
        static public ComplexClass operator -(Double v1, ComplexClass v2)
        {
            ComplexClass ret = new ComplexClass();
            ret._real = -(v2._real - v1);
            ret._imaginary = -v2._imaginary;
            return ret;
        } 
        #endregion

        static public ComplexClass operator -(ComplexClass v1)
        {
            ComplexClass ret = new ComplexClass();
            ret._real = -v1._real;
            ret._imaginary = -v1._imaginary;
            return ret;
        }


        #region Binary operator *
		static public ComplexClass operator *(ComplexClass v1, ComplexClass v2)
        {
            ComplexClass ret = new ComplexClass();
            ret._real = v1._real * v2._real - v1._imaginary*v2._imaginary;
            ret._imaginary = v1._imaginary * v2._real + v2._imaginary * v1._real;
            return ret;
        }

        static public ComplexClass operator *(ComplexClass v1, Double v2)
        {
            ComplexClass ret = new ComplexClass();
            ret._real = v1._real * v2;
            ret._imaginary = v1._imaginary * v2;
            return ret;
        }
        static public ComplexClass operator *(Double v1, ComplexClass v2)
        {
            ComplexClass ret = new ComplexClass();
            ret._real = v2._real * v1;
            ret._imaginary = v2._imaginary * v1;
            return ret;
        } 
	#endregion


        #region Binary operator /
        static public ComplexClass operator /(ComplexClass v1, ComplexClass v2)
        {
            ComplexClass ret = new ComplexClass();
            ret.Amplitude = v1.Amplitude / v2.Amplitude;
            ret.Phase = v1.Phase - v2.Phase;
            return ret;
        }
        static public ComplexClass operator /(ComplexClass v1, Double v2)
        {
            ComplexClass ret = new ComplexClass();
            ret._real = v1._real / v2;
            ret._imaginary = v1._imaginary / v2;
            return ret;
        }
        static public ComplexClass operator /(Double v1, ComplexClass v2)
        {
            ComplexClass ret = new ComplexClass();
            ret.Amplitude = v1 / v2.Amplitude;
            ret.Phase = -v2.Phase;
            return ret;
        } 
        #endregion


        //todo Операторы для Object

        static public bool operator ==(ComplexClass v1, ComplexClass v2)
        {
            if ((Object)v1 != null)
                return v1.Equals(v2);
            if ((Object)v2 != null)
                return v2.Equals(v1);
            return true;
        }

        static public bool operator !=(ComplexClass v1, ComplexClass v2)
        {
            if ((Object)v1 != null)
                return !v1.Equals(v2);
            if ((Object)v2 != null)
                return !v2.Equals(v1);
            return false;
        }
        
        #endregion

        
        #region Overrided
        #region Equals
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (base.Equals(obj))
                return true;

            ComplexClass objC = obj as ComplexClass;
            if (objC == null)
            {
                return false;
            }
            return base.Equals(obj) && (this._real == objC._real && this._imaginary == objC._imaginary);
        }
        virtual public bool Equals(ComplexClass objC)
        {

            return base.Equals(objC) && (this._real == objC._real && this._imaginary == objC._imaginary);
        } 
        #endregion
        public override string ToString()
        {
            String ret = String.Empty;

            double tempReal = Math.Round(this.Real, 2);
            ret = tempReal.ToString();

            if (this.Imaginary >= 0 || this.Imaginary.Equals(Double.NaN))
            {
                ret += " +";
            }

            double tempImag = Math.Round(this.Imaginary, 2);

            ret += tempImag.ToString() + " " + this.ComplexSymbolToString();
            return ret;
        }



        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        
    }



    static public class MathComplex
    {
        static public ComplexClass I = new ComplexClass(0,1);
        static public ComplexClass J = new ComplexClass(0,1);
        /// <summary>
        /// Возведение комплексного числа в степень
        /// </summary>
        /// <param name="value">Комплексное число</param>
        /// <param name="pow">Целая степень</param>
        /// <returns></returns>
        static public ComplexClass Pow(ComplexClass value,Int32 pow)
        {
            ComplexClass ret = new ComplexClass();
            ret.Amplitude = Math.Pow(value.Amplitude, pow);
            ret.Phase = value.Phase * pow;
            return ret;
        }
        
        //TODO Pow для дробных степеней
        /// <summary>
        /// Возведение комплексного числа в степень
        /// </summary>
        /// <param name="value">Комплексное число</param>
        /// <param name="pow">Дробная степень</param>
        /// <returns>Возвращает массив комплексных чисел</returns>
        static public ComplexClass[] Pow(ComplexClass value, Double pow)
        {
            throw new Exception("Метод не реализован");
            ComplexClass[] ret = null;
            return ret;
        }

        /// <summary>
        /// Модуль комплексного числа
        /// </summary>
        /// <param name="value">Комплексное число</param>
        /// <returns></returns>
        static public Double Abs(ComplexClass value)
        {
            return value.Amplitude;
        }
        /// <summary>
        /// Возведение экспоненты в комплексную степень
        /// </summary>
        /// <param name="value">Комплексная степень</param>
        /// <returns></returns>
        static public ComplexClass Exp(ComplexClass value)
        {
            //ComplexClass ret = new ComplexClass();
            //ret.Amplitude = Math.Exp(value.Real);
            //ret.Phase = value.Imaginary;
            //return ret;
            return ComplexClass.Exp(value);
        }
        /// <summary>
        /// Натуральный логарифм комплексного числа
        /// </summary>
        /// <param name="value">Комплексное число</param>
        /// <returns></returns>
        static public ComplexClass Ln(ComplexClass value)
        {
            ComplexClass ret = new ComplexClass();
            ret.Real = Math.Log(value.Amplitude,Math.E);
            ret.Imaginary = value.Phase;
            return ret;
        }

        /// <summary>
        /// Взятие аргумента комплексного числа
        /// </summary>
        /// <param name="value">Комплексное число</param>
        /// <returns></returns>
        static public Double Arg(ComplexClass value)
        {
            return value.Phase;
        }

        static public ComplexClass Conj(ComplexClass value)
        {
            return value.Conj;
        }
        static public ComplexClass ToComplex(String value)
        {
            throw new Exception("Функция не реализована");
            value = value.ToLower();
            
            //String rotated=value.ro
            if (value.StartsWith("-"))
            {
            }
                
            String  reals = value.Remove(value.IndexOfAny(new char[] {'+','-'}));

            String  imags = value.Substring(value.IndexOfAny(new char[] {'+','-'}));
           
            imags = imags.Substring(value.IndexOfAny(new char[] { 'i', 'j' }));
            
            Double real = Convert.ToDouble(reals);
            Double imag = Convert.ToDouble(imags);
            ComplexClass ret = new ComplexClass(real, imag);
            return ret;
        }

        #region Комплексное одномерное БПФ
        /// <summary>
        /// Комплексное одномерное БПФ 
        /// !!!Внимание!!!
        /// При количестве точек отличного от степени двойки масив дополняется нулями, что отрицательно сказывается
        /// на результатах. Рекомендуется использовать только количество точек равных степени двойки.
        /// </summary>
        /// <param name="dir">Направление преобразования (0 - прямое, 1 - обратное)</param>
        /// <param name="x">Массив комплексных чисел</param>
        /// <returns></returns> 
        static public ComplexClass[] FFT(int dir, ComplexClass[] x)
        {
            ComplexClass[] ret;
            //Сделаем колво точек чуть больше, шоб степенью двойки точно было
            long m = Convert.ToInt32(Math.Floor(Math.Log(x.Length, 2)));
            if (Math.Pow(2, m) < x.Length)
                m++;
            ret=new ComplexClass[Convert.ToInt32(Math.Pow(2,m))];
            //Инициализируем нулями
            for (int ii = 0; ii < ret.Length; ii++)
                ret[ii] = new ComplexClass();
            //Скопиркаем
            x.CopyTo(ret, ret.Length/2-x.Length/2);
           long i, i1, i2,j, k, l, l1, l2, n;
           ComplexClass tx, t1, u, c;
            //Коликчество точек
           n = ret.Length;
           i2 = n >> 1;
           j = 0;

           for (i = 0; i < n - 1; i++)
           {
               if (i < j)
               {
                   ComplexClass temp;
                   temp = ret[i];
                   ret[i] = ret[j];
                   ret[j] = temp;
               }
               k = i2;
               while (k <= j)
               {
                   j -= k;
                   k >>= 1;
               }
               j += k;
           }
           
           using (c = new ComplexClass(-1, 0))
           {
               l2 = 1;
               for (l = 0; l < m; l++)
               {
                   l1 = l2;//step
                   l2 <<= 1;//increment
                   using (u = new ComplexClass(1, 0))
                   {
                       for (j = 0; j < l1; j++)
                       {
                           for (i = j; i < n; i += l2)
                           {
                               i1 = i + l1;
                               Butterfly(Convert.ToInt32(l2), Convert.ToInt32(j), ref ret[i], ref ret[i1],dir);
                           }
                       }
                   }
                   if (dir == 1)
                       c.Imaginary=(-c.Imaginary);
               }
               if (dir == 1)
               {
                   for (i = 0; i < n; i++)
                       ret[i]=ret[i] / n;
               }
           }
           return ret;
       }
       #endregion

        #region Комплексное двумерное быстрое преобразование Фурье
        static public ComplexClass[,] FFT2(int dir, ComplexClass[,] x2D)
        {
           Int32 iNum = x2D.GetLength(0);
           Int32 jNum = x2D.GetLength(1);
           ComplexClass[,] ret;
           ComplexClass[][] retjaged = new ComplexClass[x2D.GetLength(0)][];
           for (int i = 0; i < retjaged.Length; i++)
           {
               retjaged[i] = new ComplexClass[x2D.GetLength(1)];
               for (int j = 0; j < retjaged[i].Length; j++)
               {
                   retjaged[i][j] = new ComplexClass();
                   if (i < x2D.GetLength(0) && j < x2D.GetLength(1))
                   {
                       retjaged[i][j] = x2D[i, j];
                   }
               }
           }

           for (int i = 0; i < retjaged.GetLength(0); i++)
           {
               retjaged[i] = FFT(dir, retjaged[i]);
           }

           //transpose

           ComplexClass[][] ret2 = new ComplexClass[retjaged[0].Length][];
           for (int i = 0; i < ret2.Length; i++)
           {
               ret2[i] = new ComplexClass[retjaged.Length];
               for (int j = 0; j < ret2[i].Length; j++)
               {
                   ret2[i][j] = retjaged[j][i];
               }
           }

           for (int i = 0; i < ret2.Length; i++)
           {
               ret2[i] = FFT(dir, ret2[i]);
           }
           retjaged = new ComplexClass[ret2[0].Length][];
           for (int i = 0; i < ret2.Length; i++)
           {
               retjaged[i] = new ComplexClass[ret2.Length];
               for (int j = 0; j < ret2[i].Length; j++)
               {
                   retjaged[i][j] = ret2[j][i];
               }
           }


           ret = new ComplexClass[retjaged.Length, retjaged[0].Length];
           for (int i = 0; i < retjaged.Length; i++)
           {
               for (int j = 0; j < retjaged[i].Length; j++)
               {
                   ret[i, j] = retjaged[i][j];
               }
           }
           return ret;
        } 
        #endregion

        #region Вспомогательные функции для преобразования Фурье
        static private ComplexClass W(Int32 subIndex, Int32 superIndex,int dir)
        {
            ComplexClass ret = new ComplexClass();
            if (dir == 0)
            {
                ret = MathComplex.Exp(new ComplexClass(0, -2 * Math.PI * superIndex / subIndex));
            }
            else
            {
                ret = MathComplex.Exp(new ComplexClass(0, 2 * Math.PI * superIndex / subIndex));
            }
            return ret;
        }

        static private void Butterfly(Int32 subIndex, Int32 superIndex, ref ComplexClass a, ref ComplexClass b,int dir)
        {
            ComplexClass A = new ComplexClass(a.Real, a.Imaginary);
            ComplexClass B = new ComplexClass(b.Real, b.Imaginary);
            B = W(subIndex, superIndex,dir) * B;
            a = A + B;
            b = A - B;
        } 
        #endregion

    }
}
