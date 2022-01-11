using Db;
using Esri.ArcGISRuntime.Mapping;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace El.BL
{
    public class MapVM : RootVM
    {
        public MapVM(BloggingContext efc) : base(efc)
        {
            CreateMapCommand = new RelayCommand(() => CreateMap());
        }

        private Map _WorldMap = default!;
        public Map WorldMap {
            get => _WorldMap;
            set {
                SetProperty(ref _WorldMap, value);
            }
        }

        public IRelayCommand CreateMapCommand { get; }


        private void CreateMap()
        {
            // Create new Map with basemap
            WorldMap = new Map(BasemapStyle.ArcGISStreets);
        }
    }
}
