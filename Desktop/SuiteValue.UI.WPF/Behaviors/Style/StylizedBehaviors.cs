using System.Windows;
using System.Windows.Interactivity;

namespace SuiteValue.UI.WPF.Behaviors.Style
{
    public class StylizedBehaviors
    {
        public static readonly DependencyProperty BehaviorsProperty = DependencyProperty.RegisterAttached(@"Behaviors",
                                                                                                          typeof (
                                                                                                              StylizedBehaviorCollection
                                                                                                              ),
                                                                                                          typeof (
                                                                                                              StylizedBehaviors
                                                                                                              ),
                                                                                                          new FrameworkPropertyMetadata
                                                                                                              (null,
                                                                                                               OnPropertyChanged));

        public static StylizedBehaviorCollection GetBehaviors(DependencyObject uie)
        {
            return (StylizedBehaviorCollection) uie.GetValue(BehaviorsProperty);
        }

        public static void SetBehaviors(DependencyObject uie, StylizedBehaviorCollection value)
        {
            uie.SetValue(BehaviorsProperty, value);
        }

        private static void OnPropertyChanged(DependencyObject dpo, DependencyPropertyChangedEventArgs e)
        {
            var uie = dpo as UIElement;

            if (uie == null)
            {
                return;
            }

            BehaviorCollection itemBehaviors = Interaction.GetBehaviors(uie);

            var newBehaviors = e.NewValue as StylizedBehaviorCollection;
            var oldBehaviors = e.OldValue as StylizedBehaviorCollection;

            if (newBehaviors == oldBehaviors)
            {
                return;
            }

            if (oldBehaviors != null)
            {
                foreach (var behavior in oldBehaviors)
                {
                    int index = itemBehaviors.IndexOf(behavior);

                    if (index >= 0)
                    {
                        itemBehaviors.RemoveAt(index);
                    }
                }
            }

            if (newBehaviors != null)
            {
                foreach (var behavior in newBehaviors)
                {
                    int index = itemBehaviors.IndexOf(behavior);

                    if (index < 0)
                    {
                        if (behavior.IsFrozen)
                        {
                            itemBehaviors.Add(behavior.Clone() as Behavior);
                        }
                        else
                            itemBehaviors.Add(behavior);
                    }
                }
            }
        }


    }

    public class StylizedBehaviorCollection : FreezableCollection<Behavior>
    {
        protected override Freezable CreateInstanceCore()
        {
            return new StylizedBehaviorCollection();
        }
    }
}