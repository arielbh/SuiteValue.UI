using System;
using Microsoft.Practices.Prism.Events;

namespace SuiteValue.UI.WPF.Prism.Extensions
{
    public class EventAggregatorExtensionsProvider : IEventAggregatorExtensionsProvider
    {
        public void Publish<TEvent>(IEventAggregator eventAggregator, TEvent publishEvent)
        {
            var prismEvent = eventAggregator.GetEvent<CompositePresentationEvent<TEvent>>();
            prismEvent.Publish(publishEvent);
        }

        public SubscriptionToken Subscribe<TEvent>(IEventAggregator eventAggregator, Action<TEvent> subscription)
        {
            var prismEvent = eventAggregator.GetEvent<CompositePresentationEvent<TEvent>>();
            return prismEvent.Subscribe(subscription);
        }

        public SubscriptionToken Subscribe<TEvent>(IEventAggregator eventAggregator, Action<TEvent> subscription, bool keepSubscriberReferenceAlive)
        {
            var prismEvent = eventAggregator.GetEvent<CompositePresentationEvent<TEvent>>();
            return prismEvent.Subscribe(subscription, keepSubscriberReferenceAlive);
        }

        public SubscriptionToken Subscribe<TEvent>(IEventAggregator eventAggregator, Action<TEvent> subscription, ThreadOption threadOption, bool keepSubscriberReferenceAlive = false, Predicate<TEvent> filter = null)
        {
            var prismEvent = eventAggregator.GetEvent<CompositePresentationEvent<TEvent>>();
            return prismEvent.Subscribe(subscription, threadOption, keepSubscriberReferenceAlive, filter);
        }

        public void Unsubscribe<TEvent>(IEventAggregator eventAggregator, SubscriptionToken token)
        {
            var prismEvent = eventAggregator.GetEvent<CompositePresentationEvent<TEvent>>();
            prismEvent.Unsubscribe(token);
        }

        public void Unsubscribe<TEvent>(IEventAggregator eventAggregator, Action<TEvent> subscription)
        {
            var prismEvent = eventAggregator.GetEvent<CompositePresentationEvent<TEvent>>();
            prismEvent.Unsubscribe(subscription);
        }
    }
}