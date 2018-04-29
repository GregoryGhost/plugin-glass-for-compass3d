﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassModel
{
    /// <summary>
    /// Гладкий стакан.
    /// </summary>
    public class CleanGlass : IGlass, IChecker
    {
        /// <summary>
        /// Зависимые и фиксированные параметры стакана
        /// </summary>
        private IAutoCalcParams _dependencies;

        /// <summary>
        /// Постоянный угол наклона высоты стакана
        /// </summary>
        private readonly double _angleHeight = 0.0f;

        /// <summary>
        /// Отсутствует узор стакана => высота узора равна нулю 
        /// </summary>
        private readonly double _heightFaceted = 0.0f;

        /// <summary>
        /// Отсутствуют грани стакана => количество граней равно нулю
        /// </summary>
        private readonly int _countFaceted = 0;

        /// <summary>
        /// Задаваемый параметр - высота стакана
        /// </summary>
        private BorderConditions<double> _height;

        /// <summary>
        /// Задаваемый параметр - диаметр дна стакана
        /// </summary>
        private BorderConditions<double> _diameterBottom;

        /// <summary>
        /// Процент толщины дна стакана
        /// </summary>
        private readonly double _depthBottom = 7;

        /// <summary>
        /// Процент толщины стенки стакана
        /// </summary>
        private readonly double _depthSide = 2;

        /// <summary>
        /// Установление параметров гладкого стакана.
        /// </summary>
        /// <param name="diameterBottom">Диаметр дна стакана.</param>
        /// <param name="height">Высота стакана.</param>
        public CleanGlass(BorderConditions<double> diameterBottom,
            BorderConditions<double> height)
        {
            _dependencies = new DependenciesParams();
            //Фиксированные параметры
            _dependencies.AngleHeight = true;
            _dependencies.CountFaceted = true;
            _dependencies.HeightFaceted = true;
            //Зависимые параметры
            _dependencies.DepthBottom = true;
            _dependencies.DepthSide = true;
            //Задаваемые параметры
            _dependencies.Height = false;
            _dependencies.DiameterBottom = false;

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
                if (value < this._diameterBottom.Value)
                {
                    var msg = String.Format("Высота стакана = {0} " +
                        "должна быть больше либо равна " +
                            "диаметру дна стакана = {1}", value,
                                _diameterBottom.Value);

                    _isValidParams[_labelHeight] = false;

                    throw new ArgumentException(msg);
                }
                _height.Value = value;
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
                _isValidParams[_labelDiameterBottom] = true;
            }
        }

        /// <summary>
        /// Отсутствующий угол наклона высоты стакана
        /// </summary>
        public double AngleHeight
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
        /// Зависящий параметр - толщина стенки стакана
        /// </summary>
        public double DepthSide
        {
            get
            {
                return this.DiameterBottom * _depthSide / 100;
            }
            set
            {
            }
        }

        /// <summary>
        /// Зависящий параметр - толщина дна
        /// </summary>
        public double DepthBottom
        {
            get
            {
                return this.Height * _depthBottom / 100;
            }
            set
            {
            }
        }

        /// <summary>
        /// Отсутствующий узор
        /// </summary>
        public double HeightFaceted
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
        public int CountFaceted
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
        private Dictionary<string, bool> _isValidParams =
            new Dictionary<string, bool>();
        private const string _labelDiameterBottom = "DiameterBottom";
        private const string _labelHeight = "Height";

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
    }
}
