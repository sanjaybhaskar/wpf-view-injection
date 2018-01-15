using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Specialized;
using PrismDemo.Core;

namespace PrismDemo.Prism
{
    public class DependentViewRegionBehavior : RegionBehavior
    {
        public const string BehaviorKey = "DependentViewRegionBehavior";

        protected override void OnAttach()
        {
            Region.ActiveViews.CollectionChanged += ActiveViews_CollectionChanged;
        }

        private void ActiveViews_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var tabList = new List<DependentViewInfo>();

                foreach (var newView in e.NewItems)
                {
                    foreach (var atr in GetCustomAttributes<DependentViewAttribute>(newView.GetType()))
                    {
                        var info = CreateDependentViewInfo(atr);

                        if (info.View is ISupportDataContext && newView is ISupportDataContext)
                            ((ISupportDataContext)info.View).DataContext = ((ISupportDataContext)newView).DataContext;

                        tabList.Add(info);
                    }

                    tabList.ForEach(x => Region.RegionManager.Regions[x.TargetRegionName].Add(x.View));
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                //TODO: cache so we can improve perf and remove views from region
            }
        }

        private DependentViewInfo CreateDependentViewInfo(DependentViewAttribute attribute)
        {
            var info = new DependentViewInfo();

            info.TargetRegionName = attribute.TargetRegionName;

            if (attribute.Type != null)
                info.View = Activator.CreateInstance(attribute.Type);

            return info;
        }

        private static IEnumerable<T> GetCustomAttributes<T>(Type type)
        {
            return type.GetCustomAttributes(typeof(T), true).OfType<T>();
        }
    }

    internal class DependentViewInfo
    {
        public object View { get; set; }
        public string TargetRegionName { get; set; }
    }
}
