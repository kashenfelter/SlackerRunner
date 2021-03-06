﻿using System;

namespace SlackerRunner
{
  /// <summary>
  /// Holds parsed Slacker results
  /// </summary>
  public class SlackerResults
  {
    // Test info
    public string Header { get; set; }
    public string Message { get; set; }
    public string Trace { get; set; }
    public double Seconds { get; set; }
    public int FailedSpecs { get; set; }
    public int PassedSpecs { get; set; }
    public bool Passed { get; set; }
    public string File { get; set; }


    /// <summary>
    /// Constructor in case we want to initialize any properties
    /// </summary>
    public SlackerResults()
    {
    }

    public override string ToString()
    {
      return File;
    }

    /// <summary>
    /// Returns summary of the tests parsed
    /// </summary>
    /// <returns></returns>
    public string getString()
    {
      return this.Header + Environment.NewLine +
        this.Message + Environment.NewLine +
        this.Trace + Environment.NewLine +
        "Passed: " + this.Passed + Environment.NewLine +
        "Passed Specs: " + this.PassedSpecs + Environment.NewLine +
        "Failed Specs: " + this.FailedSpecs + Environment.NewLine +
        "Seconds: " + this.Seconds + Environment.NewLine;
    }
  }
}