using EPCalipersWinUI3.Models.Calipers;
using Xunit;

namespace EPCalipersWinUi3Tests.Tests
{
	public class CalibrationTests
	{
		[Fact]
		public void TestParseCalibrationInput()
		{
			var input1 = new CalibrationMeasurement(100, Unit.Msec, "");
			var calibration1 = new Calibration(5.0, input1);
			Assert.Equal(20.0, calibration1.Multiplier);
			var input2 = new CalibrationMeasurement(100, Unit.Custom, "rods");
			var calibration2 = new Calibration(5.0, input2);
			Assert.Equal("200 rods", calibration2.GetFormattedMeasurement(10, false));
			var input3 = new CalibrationMeasurement(100, Unit.Custom, "MSEC");
			var calibration3 = new Calibration(5.0, input1);
			Assert.Equal("300 bpm", calibration3.GetFormattedMeasurement(10, true));
			//Assert.Equal(CalibrationUnit.Msec , output1.Unit);
			//var input2 = new EPCalipersWinUI3.Models.Calipers.CalibrationInput(10.0, CalibrationUnit.Uncalibrated, "");
			//CalibrationParameters output2 = Calibration.ParseInput(input2);
			//Assert.Equal(10 , output2.Value);
			//Assert.Equal(CalibrationUnit.Uncalibrated , output2.Unit);
			//var input3 = new EPCalipersWinUI3.Models.Calipers.CalibrationInput(0, CalibrationUnit.Custom, "1000 msec");
			//CalibrationParameters output3 = Calibration.ParseInput(input3);
			//Assert.Equal("msec", output3.UnitString);
			//Assert.Equal(1000 , output3.Value);
			//Assert.Equal(CalibrationUnit.Msec , output3.Unit);
			//var input4 = new EPCalipersWinUI3.Models.Calipers.CalibrationInput(0, CalibrationUnit.Custom, "1.0 sec");
			//CalibrationParameters output4 = Calibration.ParseInput(input4);
			//Assert.Equal("sec", output4.UnitString);
			//Assert.Equal(1 , output4.Value);
			//Assert.Equal(CalibrationUnit.Sec , output4.Unit);
			//var input5 = new EPCalipersWinUI3.Models.Calipers.CalibrationInput(0, CalibrationUnit.Custom, "10 mm");
			//CalibrationParameters output5 = Calibration.ParseInput(input5);
			//Assert.Equal("mm", output5.UnitString);
			//Assert.Equal(10 , output5.Value);
			//Assert.Equal(CalibrationUnit.Mm , output5.Unit);
			//var input6 = new EPCalipersWinUI3.Models.Calipers.CalibrationInput(0, CalibrationUnit.Custom, "5 mV");
			//CalibrationParameters output6 = Calibration.ParseInput(input6);
			//Assert.Equal("mV", output6.UnitString);
			//Assert.Equal(5 , output6.Value);
			//Assert.Equal(CalibrationUnit.Mv , output6.Unit);
		}

		[Fact]
		public void TestCalibrationMultiplier()
		{
			var input1 = new CalibrationMeasurement(1000, Unit.Msec, "msec");
			var value1 = 100;
			var calibration1 = new Calibration(value1, input1);
			var multiplier1 = calibration1.Multiplier;
			Assert.Equal(10.0, multiplier1, 0.0001);
			var input2 = new CalibrationMeasurement(5, Unit.Sec, "sec");
			var value2 = 100;
			var calibration2 = new Calibration(value2, input2);
			var multiplier2 = calibration2.Multiplier;
			Assert.Equal(0.05, multiplier2, 0.0001);
		}

		[Fact]
		public void TestCalibrationText()
		{
			var input1 = new CalibrationMeasurement(1000, Unit.Msec, "msec");
			var value1 = 100;
			var calibration1 = new Calibration(value1, input1);
			var unit1 = calibration1.CalibrationMeasurment.UnitString;
			Assert.Equal("msec", unit1);
			Assert.Equal("1000 msec", calibration1.GetFormattedMeasurement(value1));
		}


		[Theory]
		[InlineData("ms")]
		[InlineData("MS")]
		[InlineData("Msec")]
		public void TestIsMsec(string input)
		{
			var result = Calibration.IsMillisecondsUnit(input);
			Assert.True(result);
		}

		[Theory]
		[InlineData("mm")]
		[InlineData("MV")]
		[InlineData("sec")]
		public void TestIsNotMsec(string input)
		{
			var result = Calibration.IsMillisecondsUnit(input);
			Assert.False(result);
		}

		[Theory]
		[InlineData("mm")]
		[InlineData("millim")]
		[InlineData("MM")]
		public void TestIsMm(string input)
		{
			var result = Calibration.IsMillimetersUnit(input);
			Assert.True(result);
		}

		[Theory]
		[InlineData("s")]
		[InlineData("sec")]
		[InlineData("S")]
		public void TestIsSec(string input)
		{
			var result = Calibration.IsSecondsUnit(input);
			Assert.True(result);
		}

		[Theory]
		[InlineData("mv")]
		[InlineData("mV")]
		[InlineData("milliV")]
		[InlineData("milliVolt")]
		[InlineData("millivolts")]
		public void TestIsMv(string input)
		{
			var result = Calibration.IsMillivoltsUnit(input);
			Assert.True(result);
		}

		[Fact]
		public void IsMillisecondsUnit_ValidInput_ReturnsTrue()
		{
			// Arrange
			var input = "ms";

			// Act
			var result = Calibration.IsMillisecondsUnit(input);

			// Assert
			Assert.True(result);
		}

		[Fact]
		public void IsMillisecondsUnit_InvalidInput_ReturnsFalse()
		{
			// Arrange
			var input = "invalid";

			// Act
			var result = Calibration.IsMillisecondsUnit(input);

			// Assert
			Assert.False(result);
		}

		[Fact]
		public void TestBpm()
		{
			var input = new CalibrationMeasurement(1000, Unit.Msec, "msec");
			var value = 100;
			var calibration1 = new Calibration(value, input);

			var result = calibration1.GetFormattedMeasurement(100, false);

			Assert.Equal("1000 msec", result);

			result = calibration1.GetFormattedMeasurement(100, true);

			Assert.Equal("60 bpm", result);
		}

		[Fact]
		public void TestNoNegativeBpm()
		{
			var input = new CalibrationMeasurement(1000, Unit.Msec, "msec");
			var value = 100;
			var calibration1 = new Calibration(value, input);

			var result = calibration1.GetFormattedMeasurement(-100, false);

			Assert.Equal("-1000 msec", result);

			result = calibration1.GetFormattedMeasurement(-100, true);

			Assert.Equal("60 bpm", result);

			input = new CalibrationMeasurement(1.0, Unit.Sec, "sec");
			value = 100;
			calibration1 = new Calibration(value, input);

			result = calibration1.GetFormattedMeasurement(-100, false);

			Assert.Equal("-1.00 sec", result);

			result = calibration1.GetFormattedMeasurement(-100, true);

			Assert.Equal("60 bpm", result);
		}

		[Fact]
		public void TestUncalibrated()
		{
			var calibration = Calibration.Uncalibrated;
			Assert.Equal(Unit.Uncalibrated, calibration.CalibrationMeasurment.Unit);
		}

		[Fact]
		public void TestCalibrationText2()
		{
			var parameters = new CalibrationMeasurement(500, Unit.Msec, "MSEC");
			var calibration = new Calibration(1.0, parameters);
			Assert.Equal("500 MSEC", calibration.GetFormattedMeasurement(1.0));
		}

		[Fact]
		public void TestAngleCalibration()
		{
			var angleCalibration = new AngleCalibration();
			Assert.Equal(EPCalipersWinUI3.Models.Rounding.ToInt, angleCalibration.Rounding);
			Assert.Equal("500 points", angleCalibration.GetSecondaryText(500));
			Assert.Equal("500 points", angleCalibration.GetSecondaryText(500.1234));
		}

		[Fact]
		public void TestNewMeanRateInterval()
		{
			var parameters = new CalibrationMeasurement(1000, Unit.Msec, "msec");
			var calibration = new Calibration(100, parameters);
			calibration.Rounding = EPCalipersWinUI3.Models.Rounding.ToInt;
			Assert.Equal(1000.0, calibration.CalibratedInterval(100.0).Value);
			var meanInterval = calibration.GetMeanCalibratedInterval(500, 4, false);
			Assert.Equal("1250", meanInterval.Item1);
			var meanRate = calibration.GetMeanCalibratedInterval(500, 4, true);
			Assert.Equal("48", meanRate.Item1);
			calibration.Rounding = EPCalipersWinUI3.Models.Rounding.ToHundredths;
			meanInterval = calibration.GetMeanCalibratedInterval(500, 4, false);
			Assert.Equal("1250.00", meanInterval.Item1);
			meanRate = calibration.GetMeanCalibratedInterval(500, 4, true);
			Assert.Equal("48.00", meanRate.Item1);
			Assert.Equal("msec", meanInterval.Item2);
			Assert.Equal("bpm", meanRate.Item2);
		}

		[Fact]
		public void TestCalibrationMeasurement()
		{
			var parameters = new CalibrationMeasurement(1000, Unit.Msec, "msec");
			var calibration = new Calibration(100, parameters);
			calibration.Rounding = EPCalipersWinUI3.Models.Rounding.ToInt;
			Assert.Equal(1000.0, calibration.CalibratedInterval(100.0).Value);
			Assert.Equal(Unit.Msec, calibration.CalibratedInterval(100.0).Unit);
			Assert.Equal("msec", calibration.CalibratedInterval(100.0).UnitString);
			Assert.Equal(60.0, calibration.CalibratedInterval(100.0, true).Value);
			Assert.Equal(Unit.Msec, calibration.CalibratedInterval(100.0, true).Unit);
			Assert.Equal("bpm", calibration.CalibratedInterval(100.0, true).UnitString);
		}

		[Fact]
		public void TestMeanIntervals()
		{
			var parameters = new CalibrationMeasurement(1000, Unit.Msec, "msec");
			var calibration = new Calibration(100, parameters);
			calibration.Rounding = EPCalipersWinUI3.Models.Rounding.ToInt;
			var interval = calibration.GetMeanInterval(1200, 3);
			Assert.Equal(400, interval);
			var formattedInterval = calibration.GetNewMeanCalibratedInterval(120, 3);
			Assert.Equal("400", formattedInterval.Item1);
			Assert.Equal("msec", formattedInterval.Item2);
			formattedInterval = calibration.GetNewMeanCalibratedInterval(120, 3, true);
			Assert.Equal("150", formattedInterval.Item1);
			Assert.Equal("bpm", formattedInterval.Item2);
		}
	}
}

