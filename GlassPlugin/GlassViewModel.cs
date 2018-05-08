using GlassModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace GlassPlugin
{
    /// <summary>
    /// Помощник для работы с INotifyPropertyChanged
    /// </summary>
    public class Notify : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Оповестить об изменение свойства
        /// </summary>
        /// <param name="prop">Название свойства</param>
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }

    /// <summary>
    /// Коллекция стаканов
    /// </summary>
    public class Glasses : ObservableCollection<GlassViewModel>
    {
        private const double _min = 10;
        private const double _max = 200;
        private const double _minAngle = 0;
        private const double _maxAngle = 5;
        private const double _minDepthForFacetedGlass = 3;
        private const double _maxDepthSide = 5;
        private const double _maxDepthBottom = 7;
        private const int _minCountFaceted = 8;
        private const int _maxCountFaceted = 20;
        private const int _minCountStrips = 20;
        private const int _maxCountStrips = 60;

        /// <summary>
        /// Инициализация параметров разных типов стакана.
        /// </summary>
        public Glasses()
        {
            var height = new BorderConditions<double>(_min, _min, _max);
            var diameterBottom = new BorderConditions<double>(_min / 2,
                _min / 2, _max / 2);
            var angleHeight = new BorderConditions<double>(_minAngle,
                _minAngle, _maxAngle);
            var depthSideForFacetedGlass = new BorderConditions<double>(
                _minDepthForFacetedGlass, _minDepthForFacetedGlass,
                    _maxDepthSide);
            var depthBottom = new BorderConditions<double>(
                _minDepthForFacetedGlass, _minDepthForFacetedGlass,
                    _maxDepthBottom);
            var countFaceted = new BorderConditions<int>(
                _minCountFaceted, _minCountFaceted, _maxCountFaceted);

            var facetedGlass = new FacetedGlass(height, diameterBottom,
                angleHeight, depthSideForFacetedGlass, depthBottom,
                    countFaceted);

            var builderCleanGlass = new BuilderCleanGlass();
            var builderFacetedGlass = new BuilderFacetedGlass();
            var builderCrimpGlass = new BuilderCrimpGlass();

            Add(new GlassViewModel(facetedGlass,
                builderFacetedGlass, "Гранёный"));

            var cleanGlass = new CleanGlass(diameterBottom, height);

            countFaceted = new BorderConditions<int>(_minCountStrips,
                _minCountStrips, _maxCountStrips);
            var crimpGlass = new CrimpGlass(height,
                diameterBottom, countFaceted);

            Add(new GlassViewModel(cleanGlass,
                builderCleanGlass, "Гладкий"));
            Add(new GlassViewModel(crimpGlass,
                builderCrimpGlass, "Гофрированный"));
        }
    }

    /// <summary>
    /// Представление модели стаканов
    /// </summary>
    public class GlassesViewModel : Notify
    {
        private GlassViewModel _currentGlass;
        private Glasses _glasses;

        /// <summary>
        /// Инициализация представлений моделей стаканов.
        /// </summary>
        public GlassesViewModel()
        {
            _glasses = new Glasses();
            _currentGlass = _glasses[0];
        }

        /// <summary>
        /// Доступные стаканы
        /// </summary>
        public List<string> Names
        {
            get
            {
                return _glasses.ToList()
                               .Select(a => a.Name)
                               .ToList();
            }
        }

        /// <summary>
        /// Получить название выбранного стакана
        /// </summary>
        public string SelectedGlassName
        {
            get
            {
                return _currentGlass.Name;
            }
            set
            {
                SelectedGlass = _glasses.ToList()
                                        .First(g => g.Name == value);
                OnPropertyChanged("SelectedGlassName");
            }
        }

        /// <summary>
        /// Получить представление выбранного стакана
        /// </summary>
        public GlassViewModel SelectedGlass
        {
            get
            {
                return _currentGlass;
            }
            private set
            {
                _currentGlass = value;
                OnPropertyChanged("SelectedGlass");
            }
        }
    }

    /// <summary>
    /// Представление модели стакана
    /// </summary>
    public class GlassViewModel : Notify, IDataErrorInfo
    {
        private IGlass _glass;
        private string _name = String.Empty;
        private IChecker _checker;
        private IBuilder _builder;

        private List<Tuple<string, bool, string>> _properties;

        private readonly Tuple<string, string> _labelHeight =
            new Tuple<string, string>("Height", "Высота");
        private readonly Tuple<string, string> _labelDiameterBottom =
            new Tuple<string, string>("DiameterBottom", "Диаметр дна");
        private readonly Tuple<string, string> _labelAngleHeight =
            new Tuple<string, string>("AngleHeight", "Угол наклона высоты");
        private readonly Tuple<string, string> _labelDepthSide =
            new Tuple<string, string>("DepthSide", "Глубина стенки (в %)");
        private readonly Tuple<string, string> _labelDepthBottom =
            new Tuple<string, string>("DepthBottom", "Глубина дна (в %)");
        private readonly Tuple<string, string> _labelCountFaceted =
            new Tuple<string, string>("CountFaceted", "Количество граней");
        private readonly Tuple<string, string> _labelHeightFaceted =
            new Tuple<string, string>("HeightFaceted", "Высота узора");

        /// <summary>
        /// Инициализация представление стакана
        /// </summary>
        /// <param name="glass">Стакан</param>
        /// <param name="name">Название стакана</param>
        public GlassViewModel(IGlass glass, IBuilder builder, string name)
        {
            _glass = glass;
            _name = name;
            _checker = glass as IChecker;
            _builder = builder;

            var prop = _glass.Properties;
            _properties = new List<Tuple<string, bool, string>>
            {
                new Tuple<string,bool, string>(_labelHeight.Item1,
                    prop.Height, _labelHeight.Item2),
                new Tuple<string,bool, string>(_labelDiameterBottom.Item1,
                    prop.DiameterBottom, _labelDiameterBottom.Item2),
                new Tuple<string,bool, string>(_labelAngleHeight.Item1,
                    prop.AngleHeight, _labelAngleHeight.Item2),
                new Tuple<string,bool, string>(_labelDepthSide.Item1,
                    prop.DepthSide, _labelDepthSide.Item2),
                new Tuple<string,bool, string>(_labelDepthBottom.Item1,
                    prop.DepthBottom, _labelDepthBottom.Item2),
                new Tuple<string,bool, string>(_labelCountFaceted.Item1,
                    prop.CountFaceted, _labelCountFaceted.Item2),
                new Tuple<string,bool, string>(_labelHeightFaceted.Item1,
                    prop.HeightFaceted, _labelHeightFaceted.Item2),
            };
        }

        /// <summary>
        /// Высота стакана.
        /// </summary>
        public double Height
        {
            get
            {
                return _glass.Height;
            }
            set
            {
                _glass.Height = value;
                OnPropertyChanged(_labelHeight.Item1);
            }
        }

        /// <summary>
        /// Диаметр дна стакана.
        /// </summary>
        public double DiameterBottom
        {
            get
            {
                return _glass.DiameterBottom;
            }
            set
            {
                _glass.DiameterBottom = value;
                OnPropertyChanged(_labelDiameterBottom.Item1);
            }
        }

        /// <summary>
        /// Угол наклона высоты стакана.
        /// </summary>
        public double AngleHeight
        {
            get
            {
                return _glass.AngleHeight;
            }
            set
            {
                _glass.AngleHeight = value;
                OnPropertyChanged(_labelAngleHeight.Item1);
            }
        }

        /// <summary>
        /// Толщина стенки стакана (в процентах).
        /// </summary>
        public double DepthSide
        {
            get
            {
                return _glass.DepthSide;
            }
            set
            {
                _glass.DepthSide = value;
                OnPropertyChanged(_labelDepthSide.Item1);
            }
        }

        /// <summary>
        /// Толщина дна стакана (в процентах).
        /// </summary>
        public double DepthBottom
        {
            get
            {
                return _glass.DepthBottom;
            }
            set
            {
                _glass.DepthBottom = value;
                OnPropertyChanged(_labelDepthBottom.Item1);
            }
        }

        /// <summary>
        /// Количество граней стакана.
        /// </summary>
        public int CountFaceted
        {
            get
            {
                return _glass.CountFaceted;
            }
            set
            {
                _glass.CountFaceted = value;
                OnPropertyChanged(_labelCountFaceted.Item1);
            }
        }

        /// <summary>
        /// Высота узора стакана.
        /// </summary>
        public double HeightFaceted
        {
            get
            {
                return _glass.HeightFaceted;
            }
            set
            {
                _glass.HeightFaceted = value;
                OnPropertyChanged(_labelHeightFaceted.Item1);
            }
        }

        string IDataErrorInfo.Error { get { return null; } }

        string IDataErrorInfo.this[string column]
        {
            get
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// Узнать автовычислимые параметры стакана.
        /// </summary>
        public List<Tuple<string, bool, string>> Properties
        {
            get
            {
                return _properties;
            }
        }

        /// <summary>
        /// Получить название стакана.
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
        }

        /// <summary>
        /// Проверить валидность параметров стакана.
        /// </summary>
        public bool IsValid
        {
            get
            {
                return _checker.IsValid;
            }
        }

        /// <summary>
        /// Построение стакана в САПР.
        /// </summary>
        /// <returns>Возвращает True в случае успешного построения
        ///     и False, если построение провалилось.</returns>
        public bool BuildModel()
        {
            if (IsValid)
            {
                try
                {
                    _builder.Build(_glass, _checker);
                }
                catch (ArgumentException)
                {
                    return false;
                }
            }

            return IsValid;
        }
    }
}
