﻿using System;
using Susanoo.Command;
using Susanoo.Pipeline;

namespace Susanoo.Processing
{
    /// <summary>
    /// Shared members for all CommandBuilder processors.
    /// </summary>
    /// <typeparam name="TFilter">The type of the filter.</typeparam>
    public interface ICommandProcessorInterop<in TFilter> : ICommandProcessorInterop
    {
        /// <summary>
        /// Gets the CommandBuilder information.
        /// </summary>
        /// <value>The CommandBuilder information.</value>
        ICommandBuilderInfo<TFilter> CommandBuilderInfo { get; }
    }

    /// <summary>
    /// Shared members for all CommandBuilder processors.
    /// </summary>
    public interface ICommandProcessorInterop : IFluentPipelineFragment
    {
        /// <summary>
        /// Gets or sets the timeout of a command execution.
        /// </summary>
        /// <value>The timeout.</value>
        TimeSpan Timeout { get; set; }
    }
}