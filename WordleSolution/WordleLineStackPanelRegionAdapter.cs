using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Wordle.Controls;
using System.Collections.Specialized;

namespace Wordle
{
    internal class WordleLineStackPanelRegionAdapter : RegionAdapterBase<WordleLineStackPanel>
    {
        public WordleLineStackPanelRegionAdapter(IRegionBehaviorFactory regionBehaviorFactory) : base(regionBehaviorFactory)
        {

        }

        protected override void Adapt(IRegion region, WordleLineStackPanel regionTarget)
        {
            region.Views.CollectionChanged += (s, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    foreach (UIElement child in e.NewItems!)
                        regionTarget.Children.Add(child);
                }
                else if(e.Action == NotifyCollectionChangedAction.Remove)
                {
                    foreach (UIElement child in e.OldItems!)
                        regionTarget.Children.Remove(child);
                }
            };
        }

        protected override IRegion CreateRegion() => new SingleActiveRegion();
    }
}
