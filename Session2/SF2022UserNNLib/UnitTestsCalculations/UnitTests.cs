using SF2022UserNNLib;

namespace UnitTestsCalculations
{
	[TestClass]
	public sealed class UnitTests
	{
		private Calculations calc = new Calculations();

		/// <summary>
		///  Нет занятых интервалов, день свободен
		/// </summary>
		[TestMethod]
		public void AvailablePeriods_NoBusyIntervals_ReturnFullDay()
		{
			var startTimes = new TimeSpan[] { };
			var durations = new int[] { };
			var beginWorkingTime = new TimeSpan(9, 0, 0);
			var endWorkingTime = new TimeSpan(17, 0, 0);
			var consultationTime = 60;

			var result = calc.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

			CollectionAssert.AreEqual(new[] { "09:00-10:00", "10:00-11:00", "11:00-12:00", "12:00-13:00", "13:00-14:00", "14:00-15:00", "15:00-16:00", "16:00-17:00" }, result);
		}

		/// <summary>
		/// Один занятый интервал в начале дня
		/// </summary>
		[TestMethod]
		public void AvailablePeriods_BusyIntervalAtStart_ReturnFreeIntervals()
		{
			var startTimes = new[] { new TimeSpan(9, 0, 0) };
			var durations = new[] { 60 };
			var beginWorkingTime = new TimeSpan(9, 0, 0);
			var endWorkingTime = new TimeSpan(17, 0, 0);
			var consultationTime = 60;

			var result = calc.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

			CollectionAssert.AreEqual(new[] { "10:00-11:00", "11:00-12:00", "12:00-13:00", "13:00-14:00", "14:00-15:00", "15:00-16:00", "16:00-17:00" }, result);
		}

		/// <summary>
		/// Один занятый интервал в середине дня
		/// </summary>
		[TestMethod]
		public void AvailablePeriods_OneBusyInterval_ReturnFreeIntervals()
		{
			var startTimes = new[] { new TimeSpan(10, 0, 0) };
			var durations = new[] { 60 };
			var beginWorkingTime = new TimeSpan(9, 0, 0);
			var endWorkingTime = new TimeSpan(17, 0, 0);
			var consultationTime = 60;

			var result = calc.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

			CollectionAssert.AreEqual(new[] { "09:00-10:00", "11:00-12:00", "12:00-13:00", "13:00-14:00", "14:00-15:00", "15:00-16:00", "16:00-17:00" }, result);
		}

		/// <summary>
		/// Один занятый интервал в конце дня
		/// </summary>
		[TestMethod]
		public void AvailablePeriods_BusyIntervalAtEnd_ReturnFreeIntervals()
		{
			var startTimes = new[] { new TimeSpan(16, 0, 0) };
			var durations = new[] { 60 };
			var beginWorkingTime = new TimeSpan(9, 0, 0);
			var endWorkingTime = new TimeSpan(17, 0, 0);
			var consultationTime = 60;

			var result = calc.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

			CollectionAssert.AreEqual(new[] { "09:00-10:00", "10:00-11:00", "11:00-12:00", "12:00-13:00", "13:00-14:00", "14:00-15:00", "15:00-16:00" }, result);
		}

		/// <summary>
		/// Несколько занятых интервалов
		/// </summary>
		[TestMethod]
		public void AvailablePeriods_OverlappingBusyIntervals_ReturnFreeIntervals()
		{
			var startTimes = new[] { new TimeSpan(10, 0, 0), new TimeSpan(10, 30, 0) };
			var durations = new[] { 60, 30 };
			var beginWorkingTime = new TimeSpan(9, 0, 0);
			var endWorkingTime = new TimeSpan(17, 0, 0);
			var consultationTime = 30;

			var result = calc.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

			CollectionAssert.AreEqual(
				new[] { "09:00-09:30", "09:30-10:00", "11:00-11:30", "11:30-12:00", "12:00-12:30", "12:30-13:00", "13:00-13:30",
					"13:30-14:00", "14:00-14:30", "14:30-15:00", "15:00-15:30", "15:30-16:00", "16:00-16:30", "16:30-17:00" }, result);
		}

		/// <summary>
		/// Начало рабочего дня позже конца
		/// </summary>
		[TestMethod]
		public void AvailablePeriods_BeginWorkingTimeAfterEnd_ReturnError()
		{
			var startTimes = new TimeSpan[] { };
			var durations = new int[] { };
			var beginWorkingTime = new TimeSpan(17, 0, 0);
			var endWorkingTime = new TimeSpan(9, 0, 0);
			var consultationTime = 30;

			var result = calc.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

			CollectionAssert.AreEqual(new[] { "Начало рабочего дня должно быть раньше его окончания." }, result);
		}

		/// <summary>
		/// Начало или конец рабочего дня не указаны
		/// </summary>
		[TestMethod]
		public void AvailablePeriods_NullBeginTimeOrEndTime_ReturnError()
		{
			var startTimes = new TimeSpan[] { };
			var durations = new int[] { };
			var beginWorkingTime = new TimeSpan(0, 0, 0);
			var endWorkingTime = new TimeSpan(0, 0, 0);
			var consultationTime = 30;

			var result = calc.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

			CollectionAssert.AreEqual(new[] { "Начало и конец рабочего дня должны быть указаны." }, result);
		}

		/// <summary>
		/// Минимальное время меньше или равно нулю
		/// </summary>
		[TestMethod]
		public void AvailablePeriods_IncorrectConsultationTime_ReturnError()
		{
			var startTimes = new TimeSpan[] { };
			var durations = new int[] { };
			var beginWorkingTime = new TimeSpan(9, 0, 0);
			var endWorkingTime = new TimeSpan(17, 0, 0);
			var consultationTime = -10;

			var result = calc.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

			CollectionAssert.AreEqual(new[] { "Минимальное время должно быть больше нуля." }, result);
		}

		/// <summary>
		/// Массивы startTimes и durations разной длины
		/// </summary>
		[TestMethod]
		public void AvailablePeriods_ArraysLengthDifferent_ReturnError()
		{
			var startTimes = new[] { new TimeSpan(10, 0, 0) };
			var durations = new int[] { };
			var beginWorkingTime = new TimeSpan(9, 0, 0);
			var endWorkingTime = new TimeSpan(17, 0, 0);
			var consultationTime = 30;

			var result = calc.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

			CollectionAssert.AreEqual(new[] { "Массивы startTimes и durations должны быть одинаковой длины." }, result);
		}

		/// <summary>
		/// Занятые интервалы покрывают весь день
		/// </summary>
		[TestMethod]
		public void AvailablePeriods_FullDayBusy_ReturnEmptyArray()
		{
			var startTimes = new[] { new TimeSpan(9, 0, 0) };
			var durations = new[] { 480 };
			var beginWorkingTime = new TimeSpan(9, 0, 0);
			var endWorkingTime = new TimeSpan(17, 0, 0);
			var consultationTime = 30;

			var result = calc.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

			CollectionAssert.AreEqual(new string[] { }, result);
		}
	}
}
