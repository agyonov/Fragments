using Db;
using Esri.ArcGISRuntime.Mapping;
using Microsoft.Toolkit.Mvvm.Input;

namespace El.BL
{
    public class MapVM : RootVM
    {
        public MapVM(BloggingContext efc) : base(efc)
        {
            CreateMapCommand = new AsyncRelayCommand(async () => await CreateMap());
        }

        private Map _WorldMap = default!;
        public Map WorldMap {
            get => _WorldMap;
            set {
                SetProperty(ref _WorldMap, value);
            }
        }

        public IAsyncRelayCommand CreateMapCommand { get; }

        private async Task CreateMap()
        {
            // Create new Map with basemap
            var _tmp = new Map(BasemapStyle.OSMStreets);
            await _tmp.LoadAsync();

            // Set it
            WorldMap = _tmp;
        }
    }
}
