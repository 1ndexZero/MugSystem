using System;
using System.Collections;
using System.Collections.Generic;

namespace MugSystem
{
    public class PointEventList<T> : ICollection<PointEvent<T>>
    {
        public int Count => _points.Count;
        public bool IsReadOnly => false;
        public T InitValue { get; set; }
        public int LinearSearchThreshold { get; set; }

        protected List<PointEvent<T>> _points;

        public PointEvent<T> this[int index]
        {
            get
            {
                if (index < 0 || index >= _points.Count)
                    throw new ArgumentException("index out of range", "index");
                return _points[index];
            }
        }

        public PointEventList(T initValue, int linearSearchThreshold = 16)
        {
            _points = new List<PointEvent<T>>();
            InitValue = initValue;
            LinearSearchThreshold = linearSearchThreshold;
        }

        IEnumerator IEnumerable.GetEnumerator() =>
            _points.GetEnumerator();

        IEnumerator<PointEvent<T>> IEnumerable<PointEvent<T>>.GetEnumerator() =>
            _points.GetEnumerator();

        public virtual void Add(PointEvent<T> item)
        {
            if ((double)item.Time < 0)
                throw new ArgumentException("time cannot be negative", "item");

            if (_points.Count == 0) _points.Add(item);

            if (_points.Count <= LinearSearchThreshold)
            {
                for (int i = 0; i < _points.Count; i++)
                {
                    if (_points[i].Time >= item.Time)
                    {
                        _points.Insert(i, item);
                        return;
                    }
                }
                _points.Add(item);
            }

            else
            {
                int i = 0;
                int j = _points.Count - 1;

                while (i <= j)
                {
                    int m = i + (j - i) / 2;

                    if (_points[m].Time < item.Time)
                        i = m + 1;
                    else
                        j = m - 1;
                }

                _points.Insert(i, item);
            }
        }

        public virtual void RemoveAt(int index)
        {
            if (index < 0 || index >= _points.Count)
                throw new ArgumentException("index out of range", "index");

            _points.RemoveAt(index);
        }

        public virtual void Clear()
        {
            _points.Clear();
        }

        public bool Remove(PointEvent<T> item)
        {
            int? index = _points.FindIndex((i) => i == item);

            if (index == null) return false;

            RemoveAt((int)index);
            return true;
        }

        public bool Contains(PointEvent<T> item)
        {
            return _points.Contains(item);
        }

        public void CopyTo(PointEvent<T>[] array, int arrayIndex)
        {
            _points.CopyTo(array, arrayIndex);
        }

        public int FindIndexByBeatTime(BeatTime time)
        {
            if ((double)time < 0)
                throw new ArgumentException("time cannot be negative", "time");

            if (_points.Count == 0)
                return -1;

            if (_points.Count == 1)
                return time < _points[0].Time ? -1 : 0;

            if (time < _points[0].Time) return -1;

            if (_points.Count <= LinearSearchThreshold)
            {
                for (int i = 0; i < _points.Count - 1; i++)
                {
                    if (time >= _points[i].Time && time < _points[i + 1].Time)
                        return i;
                }
                return _points.Count - 1;
            }

            else
            {
                int i = 0;
                int j = _points.Count - 1;

                while (i <= j)
                {
                    int m = i + (j - i) / 2;

                    if (time >= _points[m].Time && (m == _points.Count - 1 || time < _points[m + 1].Time))
                        return m;
                    else if (time >= _points[m + 1].Time)
                        i = m + 1;
                    else
                        j = m - 1;
                }
            }

            throw new Exception("Unreachable code reached in FindIndexByBeatTime");
        }

        public int FindIndexByBeatTime(double time)
        {
            if (time < 0)
                throw new ArgumentException("time cannot be negative", "time");

            if (_points.Count == 0)
                return -1;

            if (_points.Count == 1)
                return time < (double)_points[0].Time ? -1 : 0;

            if (time < (double)_points[0].Time) return -1;

            if (_points.Count <= LinearSearchThreshold)
            {
                for (int i = 0; i < _points.Count - 1; i++)
                {
                    if (time >= (double)_points[i].Time && time < (double)_points[i + 1].Time)
                        return i;
                }
                return _points.Count - 1;
            }

            else
            {
                int i = 0;
                int j = _points.Count - 1;

                while (i <= j)
                {
                    int m = i + (j - i) / 2;

                    if (time >= (double)_points[m].Time && (m == _points.Count - 1 || time < (double)_points[m + 1].Time))
                        return m;
                    else if (time >= (double)_points[m + 1].Time)
                        i = m + 1;
                    else
                        j = m - 1;
                }
            }

            throw new Exception("Unreachable code reached in FindIndexByBeatTime");
        }

        public T GetValue(BeatTime time)
        {
            if ((double)time < 0)
                throw new ArgumentException("time cannot be negative", "time");

            return _points[FindIndexByBeatTime(time)].Value;
        }
    }

    public class BpmEventList : PointEventList<double>
    {
        public bool TimeCacheAutoUpdate { get; set; }

        private List<double> _timeCaches;

        public BpmEventList(double initValue, bool timeCacheAutoUpdate = true, int linearSearchThreshold = 16)
        : base(initValue, linearSearchThreshold)
        {
            if (initValue <= 0)
                throw new ArgumentException("BPM cannot be 0 or a negative number", "initValue");

            _timeCaches = new List<double>();

            TimeCacheAutoUpdate = timeCacheAutoUpdate;
        }

        public override void Add(PointEvent<double> item)
        {
            if ((double)item.Time < 0)
                throw new ArgumentException("time cannot be negative", "item");

            if (item.Value <= 0)
                throw new ArgumentException("BPM cannot be 0 or a negative number", "item");

            if (_points.Count == 0)
            {
                _points.Add(item);

                if (TimeCacheAutoUpdate)
                    _timeCaches.Add(60000.0 / InitValue * (double)item.Time);
            }

            if (_points.Count <= LinearSearchThreshold)
            {
                for (int i = 0; i < _points.Count; i++)
                {
                    if (_points[i].Time >= item.Time)
                    {
                        _points.Insert(i, item);
                        _timeCaches.Add(0);
                        UpdateTimeCaches(i);
                        return;
                    }
                }
                _points.Add(item);
                _timeCaches.Add(0);
                UpdateTimeCachesAuto(_points.Count - 1);
            }

            else
            {
                int i = 0;
                int j = _points.Count - 1;

                while (i <= j)
                {
                    int m = i + (j - i) / 2;

                    if (_points[m].Time < item.Time)
                        i = m + 1;
                    else
                        j = m - 1;
                }

                _points.Insert(i, item);
                _timeCaches.Add(0);
                UpdateTimeCachesAuto(i);
            }
        }

        public override void RemoveAt(int index)
        {
            base.RemoveAt(index);

            _timeCaches.RemoveAt(_points.Count - 1);
            UpdateTimeCachesAuto(index);
        }

        public override void Clear()
        {
            base.Clear();
            _timeCaches.Clear();
        }

        public double ConvertToMs(BeatTime time)
        {
            if ((double)time < 0)
                throw new ArgumentException("time cannot be negative", "time");

            int index = FindIndexByBeatTime(time);
            PointEvent<double> pointEvent = _points[index];

            if (index == -1)
                return 60000.0 / InitValue * (double)time;

            return _timeCaches[index] + 60000.0 / pointEvent.Value * (time - pointEvent.Time);
        }

        public double ConvertToMs(double time)
        {
            if (time < 0)
                throw new ArgumentException("time cannot be negative", "time");

            int index = FindIndexByBeatTime(time);
            PointEvent<double> pointEvent = _points[index];

            if (index == -1)
                return 60000.0 / InitValue * (double)time;

            return _timeCaches[index] + 60000.0 / pointEvent.Value * (time - (double)pointEvent.Time);
        }

        public double ConvertToBeatTime(double time)
        {
            if (time < 0)
                throw new ArgumentException("time cannot be negative", "time");
            
            int index = TimeCachesFindIndex(time);
            
            if (index == -1) return (double)time * InitValue / 60000.0;

            double cacheTime = _timeCaches[index];
            PointEvent<double> pointEvent = _points[index];

            return (double)pointEvent.Time + (time - cacheTime) * pointEvent.Value / 60000.0;
        }

        public void UpdateTimeCaches(int startIndex)
        {
            if (startIndex < 0 || startIndex >= _points.Count)
                throw new ArgumentException("startIndex out of range", "startIndex");

            if (startIndex == 0)
            {
                _timeCaches[0] = 60000.0 / InitValue * (double)_points[0].Time;
                startIndex++;
            }

            for (int i = startIndex; i < _points.Count; i++)
                _timeCaches[i] = 60000.0 / _points[i].Value * (_points[i].Time - _points[i - 1].Time);
        }

        private void UpdateTimeCachesAuto(int startIndex)
        {
            if (TimeCacheAutoUpdate) UpdateTimeCaches(startIndex);
        }

        private int TimeCachesFindIndex(double time)
        {
            if (time < 0)
                throw new ArgumentException("time cannot be negative", "time");

            if (_timeCaches.Count == 0)
                return -1;

            if (_timeCaches.Count == 1)
                return time < _timeCaches[0] ? -1 : 0;

            if (time < _timeCaches[0]) return -1;

            if (_timeCaches.Count <= LinearSearchThreshold)
            {
                for (int i = 0; i < _timeCaches.Count - 1; i++)
                {
                    if (time >= _timeCaches[i] && time < _timeCaches[i + 1])
                        return i;
                }
                return _timeCaches.Count - 1;
            }

            else
            {
                int i = 0;
                int j = _timeCaches.Count - 1;

                while (i <= j)
                {
                    int m = i + (j - i) / 2;

                    if (time >= _timeCaches[m] && (m == _timeCaches.Count - 1 || time < _timeCaches[m + 1]))
                        return m;
                    else if (time >= _timeCaches[m + 1])
                        i = m + 1;
                    else
                        j = m - 1;
                }
            }

            throw new Exception("Unreachable code reached in TimeCachesFindIndex");
        }
    }
}
