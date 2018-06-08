using System;
using System.Collections.Generic;

namespace GlassModel
{
    namespace Glasses
    {
        /// <summary>
        /// Гладкий стакан.
        /// </summary>
        public class CleanGlass : IGlass
        {
            /// <summary>
            /// Зависимые и фиксированные параметры стакана
            /// </summary>
            protected IAutoCalcParams _dependencies;

            /// <summary>
            /// Постоянный угол наклона высоты стакана
            /// </summary>
            private double _angleHeight = 0.0f;

            /// <summary>
            /// Отсутствует узор стакана => высота узора равна нулю 
            /// </summary>
            private double _heightFaceted = 0.0f;

            /// <summary>
            /// Отсутствуют грани стакана => количество граней равно нулю
            /// </summary>
            private int _countFaceted = 0;

            /// <summary>
            /// Задаваемый параметр - высота стакана
            /// </summary>
            protected BorderConditions<double> _height;

            /// <summary>
            /// Задаваемый параметр - диаметр дна стакана
            /// </summary>
            protected BorderConditions<double> _diameterBottom;

            /// <summary>
            /// Процент толщины дна стакана (в процентах).
            /// </summary>
            private double _percentForDepthBottom = 7;

            /// <summary>
            /// Процент толщины стенки стакана (в процентах).
            /// </summary>
            private double _percentForDepthSide = 2;

            /// <summary>
            /// Скругление дна и горлышка стакана.
            /// </summary>
            private bool _filleted;

            /// <summary>
            /// Установление параметров гладкого стакана.
            /// </summary>
            /// <param name="diameterBottom">Диаметр дна стакана.</param>
            /// <param name="height">Высота стакана.</param>
            public CleanGlass(BorderConditions<double> diameterBottom,
                BorderConditions<double> height)
            {
                //Фиксированные параметры - angle height,
                //  count faceted, height faceted.
                //Зависимые автовычисляемые параметры - depth bottom,
                //  depth side.
                //Задаваемые параметры - height, diameter bottom.
                _dependencies = new DependenciesParams(false, false,
                    true, true, true, true, true);

                this._height = height;
                this._diameterBottom = diameterBottom;
                _isValidParams.Add(_labelDiameterBottom, true);
                _isValidParams.Add(_labelHeight, true);
            }

            /// <summary>
            /// Задаваемый параметр - высота стакана
            /// </summary>
            /// <exception cref="ArgumentException">
            ///     Возникает, если устанавливаемое значение
            ///     выходит за рамки заданного интервала или
            ///     нарушается условие взаимосвязи 
            ///     с диаметром дна стакана.</exception>
            public double Height
            {
                get
                {
                    return _height.Value;
                }
                set
                {
                    try
                    {
                        if (value < this._diameterBottom.Value)
                        {
                            var msg = String.Format("Высота стакана = {0} " +
                                "должна быть больше либо равна " +
                                    "диаметру дна стакана = {1}", value,
                                        _diameterBottom.Value);

                            throw new ArgumentException(msg);
                        }

                        _height.Value = value;
                    }
                    catch (ArgumentException ex)
                    {
                        _isValidParams[_labelHeight] = false;
                        throw ex;
                    }
                    _isValidParams[_labelHeight] = true;
                }
            }

            /// <summary>
            /// Задаваемый параметр - диаметр дна стакана
            /// </summary>
            /// <exception cref="ArgumentException">
            ///     Возникает, если устанавливаемое значение
            ///     выходит за рамки заданного интервала или
            ///     нарушается условие взаимосвязи 
            ///     с высотой стакана.</exception>
            public double DiameterBottom
            {
                get
                {
                    return _diameterBottom.Value;
                }
                set
                {
                    try
                    {
                        if (value > this._height.Value)
                        {
                            var msg = String.Format("Диаметр дна стакан = {0} " +
                                "должен быть меньше либо равен " +
                                    "высоте стакана {1}", value,
                                        _height.Value);

                            _isValidParams[_labelDiameterBottom] = false;

                            throw new ArgumentException(msg);
                        }
                        _diameterBottom.Value = value;
                    }
                    catch (ArgumentException ex)
                    {
                        _isValidParams[_labelDiameterBottom] = false;
                        throw ex;
                    }
                    _isValidParams[_labelDiameterBottom] = true;
                }
            }

            /// <summary>
            /// Отсутствующий угол наклона высоты стакана
            /// </summary>
            public virtual double AngleHeight
            {
                get
                {
                    return _angleHeight;
                }
                set
                {
                }
            }

            /// <summary>
            /// Зависящий параметр - толщина стенки стакана (в процентах).
            /// </summary>
            public virtual double DepthSide
            {
                get
                {
                    return _percentForDepthSide;
                }
                set
                {
                }
            }

            /// <summary>
            /// Зависящий параметр - толщина дна (в процентах).
            /// </summary>
            public virtual double DepthBottom
            {
                get
                {
                    return _percentForDepthBottom;
                }
                set
                {
                }
            }

            /// <summary>
            /// Отсутствующий узор
            /// </summary>
            public virtual double HeightFaceted
            {
                get
                {
                    return _heightFaceted;
                }
                set
                {
                }
            }

            /// <summary>
            /// Отсутствующие грани
            /// </summary>
            public virtual int CountFaceted
            {
                get
                {
                    return _countFaceted;
                }
                set
                {
                }
            }

            /// <summary>
            /// Словарь с параметрами стакана: имя параметра и 
            ///     удовлетворяет ли он требованиям предметной области.
            /// </summary>
            protected Dictionary<string, bool> _isValidParams =
                new Dictionary<string, bool>();
            protected const string _labelDiameterBottom = "DiameterBottom";
            protected const string _labelHeight = "Height";

            /// <summary>
            /// Проверяет удовлетворяют ли параметры стакана
            ///     требованиям предметной области.
            /// </summary>
            public bool IsValid
            {
                get
                {
                    var valid = true;

                    foreach (var p in _isValidParams)
                    {
                        valid = p.Value;
                        if (valid == false) break;
                    }
                    return valid;
                }
            }

            /// <summary>
            /// Узнать автовычислимые параметры стакана
            /// </summary>
            public IAutoCalcParams Properties
            {
                get { return _dependencies; }
            }

            /// <summary>
            /// Скругление дна и горлышка стакана.
            /// </summary>
            public bool Filleted
            {
                get
                {
                    return _filleted;
                }
                set
                {
                    _filleted = value;
                }
            }
        }
    }
}
