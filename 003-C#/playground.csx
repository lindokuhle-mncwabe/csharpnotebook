
using System;
using static System.Console;

record PowerInverter(decimal MotorCapacity);

// IFs
decimal GetInitalSetting(PowerInverter inverter) {
  if (inverter.MotorCapacity >= 0.4m
    && inverter.MotorCapacity <= 0.75m) return 6.0m;
  if (inverter.MotorCapacity >= 1.5m
    && inverter.MotorCapacity <= 3.7m) return 4.0m;
  if (inverter.MotorCapacity >= 5.5m
    && inverter.MotorCapacity <= 7.5m) return 3.0m;
  if (inverter.MotorCapacity >= 11.0m
    && inverter.MotorCapacity <= 55.0m) return 2.0m;
  return 0.0m;
}

// switch expression
decimal GetInitalSettingSwitch(PowerInverter inverter) =>
  inverter.MotorCapacity switch {
    >= 0.4m and <= 0.75m => 6.0m,
    >= 1.5m and <= 3.7m => 4.0m,
    >= 5.5m and <= 7.5m => 3.0m,
    >= 11.0m and <= 55.0m => 2.0m,
    _ => 0.0m
  };

WriteLine(GetInitalSetting(new PowerInverter(0.5m)));
WriteLine(GetInitalSettingSwitch(new PowerInverter(0.5m)));
