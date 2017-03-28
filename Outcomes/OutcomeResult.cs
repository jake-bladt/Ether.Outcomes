﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ether.Outcomes;
using Ether.Outcomes.Formats;

namespace Ether.Outcomes
{
    /// <summary>
    /// OutcomeResult is a generic wrapper. It allows you to wrap your method responses in metadata, eliminating plumbing code.
    /// OutcomeResults are generated by calling Outcomes.Success() or Outcomes.Failure()
    /// </summary>
    /// <typeparam name="TValue">The type that this OutcomeResult wraps.</typeparam>
#if NET45 || NET40
    [Serializable]
#endif
    public class OutcomeResult<TValue> : IOutcome<TValue>
    {
        public bool Success { get; protected set; }
        public List<string> Messages { get; protected set; }
        public TValue Value { get; set; }
        public int? StatusCode { get; protected set; }
        public Dictionary<string, object> Keys { get; }
        public bool Failure => !Success;

        internal OutcomeResult(bool success)
        {
            Success = success;
            Messages = new List<string>();
            Value = default(TValue);
            StatusCode = null;
            Keys = new Dictionary<string, object>();
        }

        /// <summary>
        /// Used in cases where we need to create an OutcomeResult from an Outcome.
        /// This was implemented for use in Outcomes.Pipeline
        /// </summary>
        internal OutcomeResult(IOutcome<TValue> outcome)
        {
            Success = outcome.Success;
            Messages = outcome.Messages;
            Value = outcome.Value;
            StatusCode = outcome.StatusCode;
            Keys = outcome.Keys;
        }

        /// <summary>
        /// Used in cases where we need to create an OutcomeResult from an Outcome.
        /// This was implemented for use in Outcomes.Pipeline
        /// </summary>
        internal OutcomeResult(IOutcome outcome)
        {
            Success = outcome.Success;
            Messages = outcome.Messages;
            Value = default(TValue);
            StatusCode = outcome.StatusCode;
            Keys = outcome.Keys;
        }

        /// <returns>The message list, concatenated.</returns>
        public override string ToString()
        {
            return ToMultiLine(string.Empty);
        }

        /// <summary>
        /// Dumps the message list into a string, with a delimiter after each line. If no delimiter is specified, Outcome will make sure there is
        /// a space after each message.
        /// </summary>
        /// <param name="delimiter">A delimiter that goes after each string in the message list. Useful for implementing platform-appropriate line breaks.</param>
        /// <returns>The message list, concatenated.</returns>       
        public string ToMultiLine(string delimiter = null)
        {
            return MultiLineFormatter.ToMultiLine(delimiter, Messages);
        }
    }
}
