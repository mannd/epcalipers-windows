using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using EPCalipersWinUI3;
using EPCalipersWinUI3.Models.Calipers;
using EPCalipersWinUI3.Contracts;
using EPCalipersWinUI3.Helpers;
using System.Text.RegularExpressions;


namespace EPCalipersWinUi3Tests.Tests
{
	public class CalibrationTests
	{
		[Fact]
		public void TestParseCalibrationInput()
		{
			var input1 = new EPCalipersWinUI3.Models.Calipers.CalibrationInput(1.0, CalibrationUnit.Msec, "");
			CalibrationParameters output1 = Calibration.ParseInput(input1);
			Assert.Equal("msec", output1.UnitString);
			Assert.Equal(1 , output1.Value);
			Assert.Equal(CalibrationUnit.Msec , output1.Unit);
			var input2 = new EPCalipersWinUI3.Models.Calipers.CalibrationInput(10.0, CalibrationUnit.Uncalibrated, "");
			CalibrationParameters output2 = Calibration.ParseInput(input2);
			Assert.Equal("points", output2.UnitString);
			Assert.Equal(10 , output2.Value);
			Assert.Equal(CalibrationUnit.Uncalibrated , output2.Unit);
			var input3 = new EPCalipersWinUI3.Models.Calipers.CalibrationInput(0, CalibrationUnit.Custom, "1000 msec");
			CalibrationParameters output3 = Calibration.ParseInput(input3);
			Assert.Equal("msec", output3.UnitString);
			Assert.Equal(1000 , output3.Value);
			Assert.Equal(CalibrationUnit.Msec , output3.Unit);
			var input4 = new EPCalipersWinUI3.Models.Calipers.CalibrationInput(0, CalibrationUnit.Custom, "1.0 sec");
			CalibrationParameters output4 = Calibration.ParseInput(input4);
			Assert.Equal("sec", output4.UnitString);
			Assert.Equal(1 , output4.Value);
			Assert.Equal(CalibrationUnit.Sec , output4.Unit);
			var input5 = new EPCalipersWinUI3.Models.Calipers.CalibrationInput(0, CalibrationUnit.Custom, "10 mm");
			CalibrationParameters output5 = Calibration.ParseInput(input5);
			Assert.Equal("mm", output5.UnitString);
			Assert.Equal(10 , output5.Value);
			Assert.Equal(CalibrationUnit.Mm , output5.Unit);
			var input6 = new EPCalipersWinUI3.Models.Calipers.CalibrationInput(0, CalibrationUnit.Custom, "5 mV");
			CalibrationParameters output6 = Calibration.ParseInput(input6);
			Assert.Equal("mV", output6.UnitString);
			Assert.Equal(5 , output6.Value);
			Assert.Equal(CalibrationUnit.Mv , output6.Unit);
		}

		[Fact]
		public void TestCalibrationMultiplier()
		{
			var input1 = new EPCalipersWinUI3.Models.Calipers.CalibrationInput(1000, CalibrationUnit.Msec, "");
			var value1 = 100;
			var calibration1 = new Calibration(value1, input1);
			var multiplier1 = calibration1.Multiplier;
			Assert.Equal(10.0, multiplier1, 0.0001);
			var input2 = new EPCalipersWinUI3.Models.Calipers.CalibrationInput(1000, CalibrationUnit.Custom, "5.0 sec");
			var value2 = 100;
			var calibration2 = new Calibration(value2, input2);
			var multiplier2 = calibration2.Multiplier;
			Assert.Equal(0.05, multiplier2, 0.0001);
		}

		[Fact]
		public void TestCalibrationText()
		{
			var input1 = new EPCalipersWinUI3.Models.Calipers.CalibrationInput(1000, CalibrationUnit.Msec, "");
			var value1 = 100;
			var calibration1 = new Calibration(value1, input1);
			var unit1 = calibration1.Parameters.UnitString;
			Assert.Equal("msec", unit1);
			Assert.Equal("1000 msec", calibration1.Text);
		}

		[Fact]
		public void TestParseCustomString()
		{
			// Arrange
			var input1 = "1000 msec";

			// Act
			var output1 = Calibration.ParseCustomString(input1);

			// Assert
			Assert.Equal(1000, output1.Item1);
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

		// Similar test methods for other functions (IsSecondsUnit, IsMillimetersUnit, IsMillivoltsUnit)...
	}
}

