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
    internal class WordleLineItemsRegionAdapter : RegionAdapterBase<WordleLine>
    {
        public WordleLineItemsRegionAdapter(IRegionBehaviorFactory regionBehaviorFactory) : base(regionBehaviorFactory)
        {

        }


        protected override void Adapt(IRegion region, WordleLine regionTarget)
        {
            region.Views.CollectionChanged += (s, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    foreach (var item in e.NewItems!)
                        regionTarget.Items.Add(item);
                }
                else if(e.Action == NotifyCollectionChangedAction.Remove)
                {
                    foreach (var item in e.OldItems!)
                        regionTarget.Items.Remove(item);
                }
            };
        }

        protected override IRegion CreateRegion() => new AllActiveRegion();
    }
}
