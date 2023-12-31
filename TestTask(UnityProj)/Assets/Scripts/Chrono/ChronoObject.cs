using System.Collections.Generic;
using Time.Infrastructure;
using Tools;
using UnityEngine;

namespace Chrono
{
	public class ChronoObject : TimeProviderDecoratorBase, IChronoObject, IUpdatable
	{
		private List<ChronoAreaProvider> _areaProviders;

		private Transform _transform;

		public Vector2 Position => _transform.position;
		
		public ChronoObject(ITimeProvider originProvider, Transform transform)
		{
			_transform = transform;
			OriginProvider = originProvider;
			_areaProviders = new List<ChronoAreaProvider>();
			OriginProvider.ChangeTimeScaleEvent += OnOriginChangeTimeScaleEvent;
			
			UpdateTimeScale(1);
		}

		public void EnterArea(ChronoAreaProvider areaProvider)
		{
			_areaProviders.Add(areaProvider);
		}

		public void ExitArea(ChronoAreaProvider areaProvider)
		{
			_areaProviders.SmartRemove(areaProvider);
		}

		public void UpdateWork()
		{
			float areasTimeWrap = ChronoAreaProvider.BlendAreasTimeWrap(_areaProviders);
			float currentScale = areasTimeWrap * OriginProvider.TimeScale;

			if (currentScale.IsNotEquals(TimeScale)) 
				UpdateTimeScale(areasTimeWrap);
		}
		
		private void UpdateTimeScale(float areasTimeWrap)
		{
			float previousScale = TimeScale;
			DecoratorScale = areasTimeWrap;

			InvokeChangeTimeScaleEvent(previousScale, TimeScale);
		}
	}
}