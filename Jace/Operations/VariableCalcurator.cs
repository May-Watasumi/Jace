﻿using System.Collections.Generic;
using System.Globalization;

namespace Jace.Operations
{
    public class VariableCalcurator : Operation
    {
        static public IDictionary<string, VariableCalcurator> defaultVariables = null;

        protected Dictionary<string, VariableCalcurator> arrayInstance;// = new Dictionary<string, VariableCalcurator>();

        public string paramString;
        public VariableCalcurator indexVar;
        public bool lastResult = false;

        public VariableCalcurator(DataType dataType, string param)
            : base(dataType, true, false)
        {
            paramString = param;
//            this.DataType = type;
        }

        public VariableCalcurator(object param) : base(DataType.Array, true, false) { paramString = param.ToString(); }
        public VariableCalcurator(bool param) : base(DataType.Boolean , true, false) { paramString = param.ToString(); }
        public VariableCalcurator(int param) : base(DataType.Integer, true, false) { paramString = param.ToString(); }
        public VariableCalcurator(float param) : base(DataType.FloatingPoint, true, false) { paramString = param.ToString(); }
        public VariableCalcurator(string param) : base(DataType.Literal, true, false) { paramString = param; }
        public VariableCalcurator(uint param) : base(DataType.UnsighnedInteger, true, false) { paramString = param.ToString(); }

        protected VariableCalcurator GetArray(VariableCalcurator index)
        {
            return arrayInstance[index.Literal()];
        }

        public VariableCalcurator GetInstance(IDictionary<string, VariableCalcurator> variables = null)
        {
            if (variables == null)
                variables = defaultVariables;

            if (DataType == DataType.Identifier)
                return variables[paramString].GetInstance(variables);
            else if (DataType == DataType.Array)
                return arrayInstance[indexVar.Literal()].GetInstance(variables);
            else
                return this;
        }


        public bool Substitution(VariableCalcurator dest, IDictionary<string, VariableCalcurator> variables = null)
        {
            if (DataType != DataType.Identifier && DataType != DataType.Array)
                return false;

            if (DataType == DataType.Identifier)
                variables[paramString] = dest;
            else if (DataType == DataType.Array)
            {
                if(arrayInstance == null)
                    arrayInstance = new Dictionary<string, VariableCalcurator>();

                arrayInstance[indexVar.Literal()] = dest;
            }

            return true;
        }

        public bool Index(VariableCalcurator dest)
        {
            if (DataType != DataType.Array)
                return false;

            indexVar = dest;

            return true;
        }


        public bool Bool(bool defaultValue = false)
        {
            switch (DataType)
            {
                case DataType.Boolean:
                    bool result = false;
                    return (lastResult = bool.TryParse(paramString, out result)) ? result : defaultValue;
                case DataType.FloatingPoint:
                    float _float;
                    return float.TryParse(paramString, out _float) ? _float == 0.0f : false;
                //                case DataType.Identifier:
                case DataType.Integer:
                    int _int;
                    return int.TryParse(paramString, out _int) ? _int == 0 : false;
                case DataType.Literal:
                    return !string.IsNullOrEmpty(paramString);
                case DataType.UnsighnedInteger:
                    uint _uint;
                    return uint.TryParse(paramString, out _uint) ? _uint == 0 : false;
            }

            return false;
        }
        public float Float(float defaultValue = 0.0f)
        {
            switch (DataType)
            {
//                case DataType.Boolean:
//                    bool result = false;
//                    return (lastResult = bool.TryParse(paramString, out result)) ? result : defaultValue;
                case DataType.FloatingPoint:
                    float _float;
                    return float.TryParse(paramString, out _float) ? _float : 0.0f;
                //                case DataType.Identifier:
                case DataType.Integer:
                    int _int;
                    return int.TryParse(paramString, out _int) ? _int : 0.0f;
 //               case DataType.Literal:
 //                   return !string.IsNullOrEmpty(paramString);
                case DataType.UnsighnedInteger:
                    uint _uint;
                    return uint.TryParse(paramString, out _uint) ? _uint : 0.0f;
            }

            return 0.0f;
        }

        public int Int(int defaultValue = 0)
        {
            switch (DataType)
            {
                //                case DataType.Boolean:
                //                    bool result = false;
                //                    return (lastResult = bool.TryParse(paramString, out result)) ? result : defaultValue;
                case DataType.FloatingPoint:
                    float _float;
                    return float.TryParse(paramString, out _float) ? (int)_float : 0;
                //                case DataType.Identifier:
                case DataType.Integer:
                    int _int;
                    return int.TryParse(paramString, out _int) ? _int : 0;
                //               case DataType.Literal:
                //                   return !string.IsNullOrEmpty(paramString);
                case DataType.UnsighnedInteger:
                    uint _uint;
                    return uint.TryParse(paramString, out _uint) ? (int)_uint : 0;
            }

            return 0;
        }

        public uint Uint(uint defaultValue = 0)
        {
            switch (DataType)
            {
                //                case DataType.Boolean:
                //                    bool result = false;
                //                    return (lastResult = bool.TryParse(paramString, out result)) ? result : defaultValue;
                case DataType.FloatingPoint:
                    float _float;
                    return float.TryParse(paramString, out _float) ? (uint)_float : 0;
                //                case DataType.Identifier:
                case DataType.Integer:
                    int _int;
                    return int.TryParse(paramString, out _int) ? (uint)_int : 0;
                //               case DataType.Literal:
                //                   return !string.IsNullOrEmpty(paramString);
                case DataType.UnsighnedInteger:
                    uint _uint;
                    return uint.TryParse(paramString, out _uint) ? _uint : 0;
            }

            return 0;
        }

        public string Literal(string defaultValue = null)
        {
            return paramString;
        }

        public static VariableCalcurator operator +(VariableCalcurator src, VariableCalcurator dest)
        {
            switch (src.DataType)
            {
                case DataType.Boolean:
                    return new VariableCalcurator(true);
                case DataType.Integer:
                    return new VariableCalcurator(src.Int() + dest.Int());
                case DataType.FloatingPoint:
                    return new VariableCalcurator(src.Float() + dest.Float());
                case DataType.UnsighnedInteger:
                    return new VariableCalcurator(src.Uint() + dest.Uint());
                case DataType.Literal:
                    return new VariableCalcurator(src.Literal() + dest.Literal());
                default:
                    throw new VariableNotDefinedException(string.Format("this '+'is not defined."));
            }
        }

        public static VariableCalcurator operator -(VariableCalcurator src, VariableCalcurator dest)
        {
            switch (src.DataType)
            {
                case DataType.Boolean:
                    return new VariableCalcurator(false);
                case DataType.Integer:
                    return new VariableCalcurator(src.Int() - dest.Int());
                case DataType.FloatingPoint:
                    return new VariableCalcurator(src.Float() - dest.Float());
                case DataType.UnsighnedInteger:
                    return new VariableCalcurator(src.Uint() - dest.Uint());
                case DataType.Literal:
                    return new VariableCalcurator(string.Empty);
                default:
                    throw new VariableNotDefinedException(string.Format("this '-'is not defined."));
            }
        }

        public static VariableCalcurator operator -(VariableCalcurator src)
        {
            switch (src.DataType)
            {
                case DataType.Boolean:
                    return new VariableCalcurator(false);
                case DataType.Integer:
                    return new VariableCalcurator(src.Int() * -1);
                case DataType.FloatingPoint:
                    return new VariableCalcurator(src.Float() * -1.0f);
//                case DataType.UnsighnedInteger:
//                    return new VariableCalcurator(src.Uint() * -1);
 //               case DataType.Literal:
 //                   return new VariableCalcurator(string.Empty);
                default:
                    throw new VariableNotDefinedException(string.Format("this '-'is not defined."));
            }
        }

        public static VariableCalcurator operator *(VariableCalcurator src, VariableCalcurator dest)
        {
            switch (src.DataType)
            {
                //                case TRDataType.Bool:
                //                    return new VariableCalcurator(true);
                case DataType.Integer:
                    return new VariableCalcurator(src.Int() * dest.Int());
                case DataType.FloatingPoint:
                    return new VariableCalcurator(src.Float() * dest.Float());
                case DataType.UnsighnedInteger:
                    return new VariableCalcurator(src.Uint() * dest.Uint());
                //                case TRDataType.Literal:
                //                    return new VariableCalcurator(string.Empty);
                default:
                    throw new VariableNotDefinedException(string.Format("this '*'is not defined."));
            }
        }

        public static VariableCalcurator operator /(VariableCalcurator src, VariableCalcurator dest)
        {
            switch (src.DataType)
            {
                //               case TRDataType.Bool:
                //                    return new VariableCalcurator(true);
                case DataType.Integer:
                    return new VariableCalcurator(src.Int() / dest.Int());
                case DataType.FloatingPoint:
                    return new VariableCalcurator(src.Float() / dest.Float());
                case DataType.UnsighnedInteger:
                    return new VariableCalcurator(src.Uint() / dest.Uint());
                //                case TRDataType.Literal:
                //                    return new VariableCalcurator(src.Literal() + dest.Literal());
                default:
                    throw new VariableNotDefinedException(string.Format("this '/'is not defined."));
            }
        }

        public static VariableCalcurator operator %(VariableCalcurator src, VariableCalcurator dest)
        {
            switch (src.DataType)
            {
//                case DataType.Boolean:
//                    return new VariableCalcurator(true);
                case DataType.Integer:
                    return new VariableCalcurator(src.Int() % dest.Int());
                case DataType.FloatingPoint:
                    return new VariableCalcurator(src.Float() % dest.Float());
                case DataType.UnsighnedInteger:
                    return new VariableCalcurator(src.Uint() % dest.Uint());
//                case DataType.Literal:
//                    return new VariableCalcurator(src.Literal() + dest.Literal());
                default:
                    throw new VariableNotDefinedException(string.Format("this '%'is not defined."));
            }
        }

        public static VariableCalcurator operator &(VariableCalcurator src, VariableCalcurator dest)
        {
            switch (src.DataType)
            {
                case DataType.Boolean:
                    return new VariableCalcurator(src.Bool() && dest.Bool());
                case DataType.Integer:
                    return new VariableCalcurator(src.Int() != 0 && dest.Int() != 1);
                case DataType.FloatingPoint:
                    return new VariableCalcurator(src.Float() != 0.0f && dest.Float() != 0.0f);
                case DataType.UnsighnedInteger:
                    return new VariableCalcurator(src.Uint() != 0 && dest.Uint() != 0);
                case DataType.Literal:
                    return new VariableCalcurator(string.IsNullOrEmpty(src.Literal()) && string.IsNullOrEmpty(dest.Literal()));
                default:
                    throw new VariableNotDefinedException(string.Format("this '&&'is not defined."));
            }
        }

        public static VariableCalcurator operator |(VariableCalcurator src, VariableCalcurator dest)
        {
            switch (src.DataType)
            {
                case DataType.Boolean:
                    return new VariableCalcurator(src.Bool() || dest.Bool());
                case DataType.Integer:
                    return new VariableCalcurator((src.Int() != 0) || (dest.Int() != 1));
                case DataType.FloatingPoint:
                    return new VariableCalcurator((src.Float() != 0.0f) || (dest.Float() != 0.0f));
                case DataType.UnsighnedInteger:
                    return new VariableCalcurator((src.Uint() != 0) || (dest.Uint() != 0));
                case DataType.Literal:
                    return new VariableCalcurator(string.IsNullOrEmpty(src.Literal()) || string.IsNullOrEmpty(dest.Literal()));
                default:
                    throw new VariableNotDefinedException(string.Format("this '||'is not defined."));
            }
        }

        public static VariableCalcurator operator ==(VariableCalcurator src, VariableCalcurator dest)
        {
            switch (src.DataType)
            {
                case DataType.Boolean:
                    return new VariableCalcurator(src.Bool() == dest.Bool());
                case DataType.Integer:
                    return new VariableCalcurator((src.Int() != 0) == (dest.Int() != 1));
                case DataType.FloatingPoint:
                    return new VariableCalcurator((src.Float() != 0.0f) == (dest.Float() != 0.0f));
                case DataType.UnsighnedInteger:
                    return new VariableCalcurator((src.Uint() != 0) == (dest.Uint() != 0));
                case DataType.Literal:
                    return new VariableCalcurator(string.IsNullOrEmpty(src.Literal()) == string.IsNullOrEmpty(dest.Literal()));
                default:
                    throw new VariableNotDefinedException(string.Format("this '||'is not defined."));
            }
        }

        public static VariableCalcurator operator !=(VariableCalcurator src, VariableCalcurator dest)
        {
            switch (src.DataType)
            {
                case DataType.Boolean:
                    return new VariableCalcurator(src.Bool() != dest.Bool());
                case DataType.Integer:
                    return new VariableCalcurator((src.Int() != 0) != (dest.Int() != 1));
                case DataType.FloatingPoint:
                    return new VariableCalcurator((src.Float() != 0.0f) != (dest.Float() != 0.0f));
                case DataType.UnsighnedInteger:
                    return new VariableCalcurator((src.Uint() != 0) != (dest.Uint() != 0));
                case DataType.Literal:
                    return new VariableCalcurator(string.IsNullOrEmpty(src.Literal()) != string.IsNullOrEmpty(dest.Literal()));
                default:
                    throw new VariableNotDefinedException(string.Format("this '!='is not defined."));
            }
        }

        public static VariableCalcurator operator <=(VariableCalcurator src, VariableCalcurator dest)
        {
            switch (src.DataType)
            {
                //               case TRDataType.Bool:
                //                    return new VariableCalcurator(true);
                case DataType.Integer:
                    return new VariableCalcurator(src.Int() <= dest.Int());
                case DataType.FloatingPoint:
                    return new VariableCalcurator(src.Float() <= dest.Float());
                case DataType.UnsighnedInteger:
                    return new VariableCalcurator(src.Uint() <= dest.Uint());
                //                case TRDataType.Literal:
                //                    return new VariableCalcurator(src.Literal() + dest.Literal());
                default:
                    throw new VariableNotDefinedException(string.Format("this '<='is not defined."));
            }
        }

        public static VariableCalcurator operator >=(VariableCalcurator src, VariableCalcurator dest)
        {
            switch (src.DataType)
            {
                //               case TRDataType.Bool:
                //                    return new VariableCalcurator(true);
                case DataType.Integer:
                    return new VariableCalcurator(src.Int() >= dest.Int());
                case DataType.FloatingPoint:
                    return new VariableCalcurator(src.Float() >= dest.Float());
                case DataType.UnsighnedInteger:
                    return new VariableCalcurator(src.Uint() >= dest.Uint());
                //                case TRDataType.Literal:
                //                    return new VariableCalcurator(src.Literal() + dest.Literal());
                default:
                    throw new VariableNotDefinedException(string.Format("this '>='is not defined."));
            }
        }

        public static VariableCalcurator operator <(VariableCalcurator src, VariableCalcurator dest)
        {
            switch (src.DataType)
            {
                //               case TRDataType.Bool:
                //                    return new VariableCalcurator(true);
                case DataType.Integer:
                    return new VariableCalcurator(src.Int() < dest.Int());
                case DataType.FloatingPoint:
                    return new VariableCalcurator(src.Float() < dest.Float());
                case DataType.UnsighnedInteger:
                    return new VariableCalcurator(src.Uint() < dest.Uint());
                //                case TRDataType.Literal:
                //                    return new VariableCalcurator(src.Literal() + dest.Literal());
                default:
                    throw new VariableNotDefinedException(string.Format("this '<'is not defined."));
            }
        }

        public static VariableCalcurator operator >(VariableCalcurator src, VariableCalcurator dest)
        {
            switch (src.DataType)
            {
                //               case TRDataType.Bool:
                //                    return new VariableCalcurator(true);
                case DataType.Integer:
                    return new VariableCalcurator(src.Int() > dest.Int());
                case DataType.FloatingPoint:
                    return new VariableCalcurator(src.Float() > dest.Float());
                case DataType.UnsighnedInteger:
                    return new VariableCalcurator(src.Uint() > dest.Uint());
                //                case TRDataType.Literal:
                //                    return new VariableCalcurator(src.Literal() + dest.Literal());
                default:
                    throw new VariableNotDefinedException(string.Format("this '>'is not defined."));
            }
        }
    }
}
