using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassPlugin.Models
{
    namespace ExceptionsOfGlassParameters
    {
        [Serializable()]
        public class DifferentTopAndBottomDiameters : System.ArgumentOutOfRangeException
        {
            public DifferentTopAndBottomDiameters() : base() { }
            public DifferentTopAndBottomDiameters(string message) : base(message) { }
            public DifferentTopAndBottomDiameters(string message, System.ArgumentOutOfRangeException inner) : base(message, inner) { }

            // Конструктор для обработки сериализации типа 
            protected DifferentTopAndBottomDiameters(System.Runtime.Serialization.SerializationInfo info,
                System.Runtime.Serialization.StreamingContext context) { }
        }

        [Serializable()]
        public class UnacceptableNumberOfFaces : System.ArgumentOutOfRangeException
        {
            public UnacceptableNumberOfFaces() : base() { }
            public UnacceptableNumberOfFaces(string message) : base(message) { }
            public UnacceptableNumberOfFaces(string message, System.ArgumentOutOfRangeException inner) : base(message, inner) { }

            // Конструктор для обработки сериализации типа 
            protected UnacceptableNumberOfFaces(System.Runtime.Serialization.SerializationInfo info,
                System.Runtime.Serialization.StreamingContext context) { }
        }

        [Serializable()]
        public class OutOfRangeDepthBottom : System.ArgumentOutOfRangeException
        {
            public OutOfRangeDepthBottom() : base() { }
            public OutOfRangeDepthBottom(string message) : base(message) { }
            public OutOfRangeDepthBottom(string message, System.ArgumentOutOfRangeException inner) : base(message, inner) { }

            // Конструктор для обработки сериализации типа 
            protected OutOfRangeDepthBottom(System.Runtime.Serialization.SerializationInfo info,
                System.Runtime.Serialization.StreamingContext context) { }
        }

        [Serializable()]
        public class OutOfRangeTitleAngle : System.ArgumentOutOfRangeException
        {
            public OutOfRangeTitleAngle() : base() { }
            public OutOfRangeTitleAngle(string message) : base(message) { }
            public OutOfRangeTitleAngle(string message, System.ArgumentOutOfRangeException inner) : base(message, inner) { }

            // Конструктор для обработки сериализации типа 
            protected OutOfRangeTitleAngle(System.Runtime.Serialization.SerializationInfo info,
                System.Runtime.Serialization.StreamingContext context) { }
        }

        [Serializable()]
        public class OutOfRangeSideDepth : System.ArgumentOutOfRangeException
        {
            public OutOfRangeSideDepth() : base() { }
            public OutOfRangeSideDepth(string message) : base(message) { }
            public OutOfRangeSideDepth(string message, System.ArgumentOutOfRangeException inner) : base(message, inner) { }

            // Конструктор для обработки сериализации типа 
            protected OutOfRangeSideDepth(System.Runtime.Serialization.SerializationInfo info,
                System.Runtime.Serialization.StreamingContext context) { }
        }

        [Serializable()]
        public class OutOfRangeHeightFace: System.ArgumentOutOfRangeException
        {
            public OutOfRangeHeightFace() : base() { }
            public OutOfRangeHeightFace(string message) : base(message) { }
            public OutOfRangeHeightFace(string message, System.ArgumentOutOfRangeException inner) : base(message, inner) { }

            // Конструктор для обработки сериализации типа 
            protected OutOfRangeHeightFace(System.Runtime.Serialization.SerializationInfo info,
                System.Runtime.Serialization.StreamingContext context) { }
        }

        [Serializable()]
        public class DiameterBottomAboveHeightGlass : System.ArgumentOutOfRangeException
        {
            public DiameterBottomAboveHeightGlass() : base() { }
            public DiameterBottomAboveHeightGlass(string message) : base(message) { }
            public DiameterBottomAboveHeightGlass(string message, System.ArgumentOutOfRangeException inner) : base(message, inner) { }

            // Конструктор для обработки сериализации типа 
            protected DiameterBottomAboveHeightGlass(System.Runtime.Serialization.SerializationInfo info,
                System.Runtime.Serialization.StreamingContext context) { }
        }

        [Serializable()]
        public class DiameterTopAboveHeightGlass : System.ArgumentOutOfRangeException
        {
            public DiameterTopAboveHeightGlass() : base() { }
            public DiameterTopAboveHeightGlass(string message) : base(message) { }
            public DiameterTopAboveHeightGlass(string message, System.ArgumentOutOfRangeException inner) : base(message, inner) { }

            // Конструктор для обработки сериализации типа 
            protected DiameterTopAboveHeightGlass(System.Runtime.Serialization.SerializationInfo info,
                System.Runtime.Serialization.StreamingContext context) { }
        }

        [Serializable()]
        public class DiameterBottomOutOfRangeDiameterTop : System.ArgumentOutOfRangeException
        {
            public DiameterBottomOutOfRangeDiameterTop() : base() { }
            public DiameterBottomOutOfRangeDiameterTop(string message) : base(message) { }
            public DiameterBottomOutOfRangeDiameterTop(string message, System.ArgumentOutOfRangeException inner) : base(message, inner) { }

            // Конструктор для обработки сериализации типа 
            protected DiameterBottomOutOfRangeDiameterTop(System.Runtime.Serialization.SerializationInfo info,
                System.Runtime.Serialization.StreamingContext context) { }
        }
    }
}
