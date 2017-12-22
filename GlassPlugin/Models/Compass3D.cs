using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GlassPlugin.Models
{
    class Compass3D
    {
        //TODO: подключить либу Компаса
        //TODO: заменить класс у _kompas на KompasObject
        Compass3D _kompas;

        public void BuildGlass(Parameters glass)
        {
            double depthBottom;
            double bottomTor;

            var topTorOfGlass = glass.heightFaceOfGlass * 5 / 100;

            switch (glass.typeOfGlass)
            {
                case TypeGlass.Faceted:
                    bottomTor = glass.heightFaceOfGlass * 5 / 100;
                    topTorOfGlass = glass.heightFaceOfGlass * 5 / 100;

                    break;
                case TypeGlass.Crimp:
                    bottomTor = glass.heightGlass * 2 / 100;
                    depthBottom = glass.heightGlass * 1 / 100;

                    var bowlPattern = glass.heightGlass * 2 / 100;
                    var sideDepth = glass.heightGlass * 1 / 100;

                    break;
                case TypeGlass.Clean:
                    depthBottom = glass.heightGlass * 10 / 100;
                    sideDepth = glass.heightGlass * 3 / 100;

                    break;
            }
        }

        public void StartCompass()
        {

        }
    }
}