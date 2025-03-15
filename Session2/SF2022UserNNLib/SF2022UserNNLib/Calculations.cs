namespace SF2022UserNNLib
{
    public class Calculations
    {

		/// <summary>
		/// Возвращает список свободных временных интервалов.
		/// </summary>
		/// <param name="startTimes">Массив начал занятых промежутков</param>
		/// <param name="durations">Массив длительностей занятых промежутков</param>
		/// <param name="beginWorkingTime">Начало рабочего дня</param>
		/// <param name="endWorkingTime">Конец рабочего дня</param>
		/// <param name="consultationTime">Минимальное необходимое время для работы</param>
		/// <returns>Список свободных интервалов в формате HH:mm-HH:mm</returns>
		public string[] AvailablePeriods(TimeSpan[] startTimes, int[] durations, TimeSpan beginWorkingTime,
			TimeSpan endWorkingTime, int consultationTime)
		{
			List<string> freeIntervals = new List<string>();

			if (consultationTime <= 0)
			{
				freeIntervals.Add("Минимальное время должно быть больше нуля.");
				return freeIntervals.ToArray();
			}
			if (beginWorkingTime == new TimeSpan(0, 0, 0) || endWorkingTime == new TimeSpan(0, 0, 0))
			{
				freeIntervals.Add("Начало и конец рабочего дня должны быть указаны.");
				return freeIntervals.ToArray();
			}
			if (beginWorkingTime >= endWorkingTime)
			{
				freeIntervals.Add("Начало рабочего дня должно быть раньше его окончания.");
				return freeIntervals.ToArray();
			}
			if (startTimes.Length != durations.Length)
			{
				freeIntervals.Add("Массивы startTimes и durations должны быть одинаковой длины.");
				return freeIntervals.ToArray();
			}
			TimeSpan consultation = new TimeSpan(0, consultationTime, 0);

			// Список занятых интервалов
			var busyIntervals = new List<(TimeSpan Start, TimeSpan End)>();
			for (int i = 0; i < startTimes.Length; i++)
			{
				TimeSpan start = startTimes[i];
				TimeSpan end = start + new TimeSpan(0, durations[i], 0);
				busyIntervals.Add((start, end));
			}
			busyIntervals = busyIntervals.OrderBy(a => a.Start).ToList();
			TimeSpan currentStart = beginWorkingTime;
			foreach (var bi in busyIntervals)
			{
				if (bi.Start > currentStart)
				{
					var freeEnd = bi.Start;
					while (currentStart + consultation <= freeEnd)
					{
						var intervalEnd = currentStart + consultation;
						freeIntervals.Add($"{currentStart:hh\\:mm}-{intervalEnd:hh\\:mm}");
						currentStart = intervalEnd;
					}

				}
				currentStart = bi.End > currentStart ? bi.End : currentStart;
			}
			if (endWorkingTime - currentStart >= consultation)
			{
				while (currentStart + consultation <= endWorkingTime)
				{
					var intervalEnd = currentStart + consultation;
					freeIntervals.Add($"{currentStart:hh\\:mm}-{intervalEnd:hh\\:mm}");
					currentStart = intervalEnd;
				}
			}
			return freeIntervals.ToArray();
		}
	}
}
