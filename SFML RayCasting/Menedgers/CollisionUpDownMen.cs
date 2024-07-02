using SFML.System;
using SFML_RayCasting.Maps;
using SFML_RayCasting.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFML_RayCasting.Menedgers
{
	public static class CollisionUpDownMen
	{
		public static (float down, float up) CalczIndex(Vector2f pos, float zIndex, MapDef map)
		{
			List<AbsObject> objs = map.Objects;

			List<(float, float, AbsObject)> listZIndex = new List<(float, float, AbsObject)>(); // Добавляем верхний zIndex

			foreach (AbsObject obj in objs)
			{
				if(!obj.isCollision) continue;
				if (obj is VertexObject item)
				{
					var res = item.IsPointInside(pos);
					if (res)
					{
						float upperZIndex = obj.SizeWall > 1 ? item.zIndex + item.SizeWall : obj.SizeWall == 1 ? item.zIndex : item.zIndex - item.SizeWall; // Рассчитываем верхний zIndex с учётом высоты стены
						listZIndex.Add((item.zIndex, upperZIndex, item));
					}
				}
			}

			listZIndex.Sort((a, b) => a.Item1.CompareTo(b.Item1));
			float closestLowerValue = float.MinValue; // Инициализируем минимальным значением
			float closestUpperValue = float.MaxValue; // Инициализируем максимальным значением

			foreach (var item in listZIndex)
			{
				float lowerValue = item.Item1;
				float upperValue = item.Item2;

				if (upperValue < zIndex)
				{
					closestLowerValue = Math.Max(closestLowerValue, upperValue); // Обновляем ближайшее меньшее значение
				}
				else if (lowerValue > zIndex && lowerValue < closestUpperValue)
				{
					closestUpperValue = lowerValue; // Обновляем ближайшее большее значение
				}
			}

			// Если не найдено ни одного значения меньше zIndex, возвращаем 0 вместо float.MinValue
			if (closestLowerValue == float.MinValue)
			{
				closestLowerValue = 0;
			}

			// Если не найдено ни одного значения больше zIndex, возвращаем 0 вместо float.MaxValue
			if (closestUpperValue == float.MaxValue)
			{
				closestUpperValue = 0;
			}

			return (closestLowerValue, closestUpperValue);
		}
	}
}
