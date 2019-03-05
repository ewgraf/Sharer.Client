using System.Collections.Concurrent;

namespace Sharer.Client {
	public class FixedSizedQueue<T> {
		ConcurrentQueue<T> _queue = new ConcurrentQueue<T>();

		public int MaximumLength { get; set; }

		public void Enqueue(T obj) {
			_queue.Enqueue(obj);
			lock (this) {
				T overflow;
				while (_queue.Count > MaximumLength && _queue.TryDequeue(out overflow));
			}
		}

		public bool TryDequeue(out T param) {
			T obj;
			if(_queue.TryDequeue(out obj)) {
				param = obj;
				return true;
			}
			param = default(T);
			return false;
		}
	}
}
