﻿using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;

namespace PooledAwait
{
    /// <summary>
    /// A ValueTask with a custom source and builder
    /// </summary>
    [AsyncMethodBuilder(typeof(TaskBuilders.PooledValueTaskBuilder))]
    public readonly struct PooledValueTask
    {
        private readonly IValueTaskSource _source;
        private readonly short _token;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal PooledValueTask(IValueTaskSource source, short token)
        {
            _source = source;
            _token = token;
        }

        /// <summary>
        /// Gets the instance as a value-task
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ValueTask AsValueTask() => _source == null ? default : new ValueTask(_source, _token);

        /// <summary>
        /// Gets the instance as a value-task
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ValueTask(in PooledValueTask task) => task.AsValueTask();

        /// <summary>
        /// Gets the awaiter for the task
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ValueTaskAwaiter GetAwaiter() => AsValueTask().GetAwaiter();

        /// <summary>
        /// Gets the configured awaiter for the task
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ConfiguredValueTaskAwaitable ConfigureAwait(bool continueOnCapturedContext)
            => AsValueTask().ConfigureAwait(continueOnCapturedContext);

        /// <summary>
        /// Rents a task-source that will be recycled when the task is awaited
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static PooledValueTaskSource CreateSource() => PooledValueTaskSource.Create();
    }
}
