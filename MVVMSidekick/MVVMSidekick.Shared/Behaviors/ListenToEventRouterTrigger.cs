﻿#if !NETFX_CORE
using MVVMSidekick.EventRouting;
using MVVMSidekick.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Interactivity;
#else
using System.Reactive.Linq;
using System.Linq;
using System.Threading;
using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;
using System;
using MVVMSidekick.EventRouting;
using System.Collections.ObjectModel;
#endif

namespace MVVMSidekick.Behaviors
{

#if NETFX_CORE
	public class ListenToEventRouterTriggerBehavior : BehaviorBase

	{

		private object InvokeActions(object e)
		{
			foreach (var item in Actions)
			{
				try
				{
					item.Execute(this, e);
				}
				catch (Exception)
				{
					//throw;
				}
			}
			return e;
		}

		public ObservableCollection<Microsoft.Xaml.Interactivity.IAction> Actions
		{
			get { return (ObservableCollection<Microsoft.Xaml.Interactivity.IAction>)GetValue(ActionsProperty); }
			set { SetValue(ActionsProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Actions.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ActionsProperty =
			DependencyProperty.Register("Actions", typeof(ObservableCollection<Microsoft.Xaml.Interactivity.IAction>), typeof(ListenToEventRouterTriggerBehavior), new PropertyMetadata(new ObservableCollection<IAction>()));



	

	

		public override void Attach(DependencyObject associatedObject)
		{
			ExchangeTheSubscribedRouter(this, EventRouter);

			base.Attach(associatedObject);
		}


		
		public override void Detach()
		{
			base.Detach();
			if (_oldSubscrption != null)
			{
				_oldSubscrption.Dispose();
				_oldSubscrption = null;
			}
		}


		public EventRouter EventRouter
		{
			get { return (EventRouter)GetValue(EventRouterProperty); }
			set { SetValue(EventRouterProperty, value); }
		}

		// Using a DependencyProperty as the backing store for EventRouter.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty EventRouterProperty =
			DependencyProperty.Register("EventRouter", typeof(EventRouter), typeof(ListenToEventRouterTriggerBehavior), new PropertyMetadata(null,
				(o, e) =>
				{
					var t = o as ListenToEventRouterTriggerBehavior;
					ExchangeTheSubscribedRouter(t, e.NewValue as EventRouter);
				}));




		/// <summary>
		/// Gets or sets the type of the event object.
		/// </summary>
		/// <value>
		/// The type of the event object.
		/// </value>
		public System.Type EventObjectType
		{
			get { return (Type)GetValue(EventObjectTypeProperty); }
			set { SetValue(EventObjectTypeProperty, value); }
		}

		// Using a DependencyProperty as the backing store for EventObjectType.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty EventObjectTypeProperty =
			DependencyProperty.Register("EventObjectType", typeof(Type), typeof(ListenToEventRouterTriggerBehavior), new PropertyMetadata(typeof(object)));



		/// <summary>
		/// Gets or sets the name of the event routing.
		/// </summary>
		/// <value>
		/// The name of the event routing.
		/// </value>
		public string EventRoutingName
		{
			get { return (string)GetValue(EventRoutingNameProperty); }
			set { SetValue(EventRoutingNameProperty, value); }
		}


		// Using a DependencyProperty as the backing store for EventRoutingName.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty EventRoutingNameProperty =
			DependencyProperty.Register("EventRoutingName", typeof(string), typeof(ListenToEventRouterTriggerBehavior), new PropertyMetadata(null));




		IDisposable _oldSubscrption;

		static void ExchangeTheSubscribedRouter(ListenToEventRouterTriggerBehavior trigger, EventRouter newRouter)
		{

			var targetEventRouter = newRouter ?? EventRouter.Instance;
			var query = targetEventRouter.GetEventChannel<object>()
				.Where(x => string.IsNullOrEmpty(trigger.EventRoutingName) || trigger.EventRoutingName == x.EventName);


			var old = Interlocked.Exchange(
				ref trigger._oldSubscrption,
				query.Subscribe(e => trigger.InvokeActions(e)));

			old.Dispose();

		}

	}
#endif

#if NETFX_CORE
    public class ListenToEventRouterDataBehavior : BehaviorBase
#else 
	public class ListenToEventRouterDataBehavior : Behavior<FrameworkElement>
#endif 
	{



		public Object LastDataReceived
		{
			get { return (Object)GetValue(LastDataReceivedProperty); }
			set { SetValue(LastDataReceivedProperty, value); }
		}

		// Using a DependencyProperty as the backing store for LastDataReceived.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty LastDataReceivedProperty =
			DependencyProperty.Register("LastDataReceived", typeof(Object), typeof(ListenToEventRouterDataBehavior), new PropertyMetadata(0));


		/// <summary>
		/// Gets or sets the type of the event object.
		/// </summary>
		/// <value>
		/// The type of the event object.
		/// </value>
		public System.Type EventObjectType
		{
			get { return (Type)GetValue(EventObjectTypeProperty); }
			set { SetValue(EventObjectTypeProperty, value); }
		}

		// Using a DependencyProperty as the backing store for EventObjectType.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty EventObjectTypeProperty =
			DependencyProperty.Register("EventObjectType", typeof(Type), typeof(ListenToEventRouterDataBehavior), new PropertyMetadata(typeof(object)));


		public EventRouter EventRouter
		{
			get { return (EventRouter)GetValue(EventRouterProperty); }
			set { SetValue(EventRouterProperty, value); }
		}

		// Using a DependencyProperty as the backing store for EventRouter.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty EventRouterProperty =
			DependencyProperty.Register("EventRouter", typeof(EventRouter), typeof(ListenToEventRouterDataBehavior), new PropertyMetadata(null,
				(o, e) =>
				{
					var t = o as ListenToEventRouterDataBehavior;
					ExchangeTheSubscribedRouter(t, e.NewValue as EventRouter);
				}));




		IDisposable _oldSubscrption;

		static void ExchangeTheSubscribedRouter(ListenToEventRouterDataBehavior bhv, EventRouter newRouter)
		{

			var targetEventRouter = newRouter ?? EventRouter.Instance;
			var query = targetEventRouter.GetEventChannel<object>()
				.Where(x => string.IsNullOrEmpty(bhv.EventRoutingName) || bhv.EventRoutingName == x.EventName);


			var old = Interlocked.Exchange(
				ref bhv._oldSubscrption,
				query.Subscribe(e => bhv.LastDataReceived = e.EventData));

			old.Dispose();

		}



		/// <summary>
		/// Gets or sets the name of the event routing.
		/// </summary>
		/// <value>
		/// The name of the event routing.
		/// </value>
		public string EventRoutingName
		{
			get { return (string)GetValue(EventRoutingNameProperty); }
			set { SetValue(EventRoutingNameProperty, value); }
		}


		// Using a DependencyProperty as the backing store for EventRoutingName.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty EventRoutingNameProperty =
			DependencyProperty.Register("EventRoutingName", typeof(string), typeof(ListenToEventRouterDataBehavior), new PropertyMetadata(null));



	}

}
