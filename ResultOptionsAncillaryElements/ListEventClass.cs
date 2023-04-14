using System;
using System.Collections.Generic;
using System.Text;

using System.Collections;

namespace ResultOptionsClassLibrary
{
    [Serializable]
    public class ListEventClass<T> : IList<T>
    {
        public ListEventClass()
        {
            MyList = new List<T>();
        }
        
        protected IList<T> MyList = null;

        public delegate void ChangeItemsInListDelegate();

        public event ChangeItemsInListDelegate ChangeItemsInListEvent;

        protected void SendChangeItemsInListEvent()
        {
            #region Рассылка EventSender

            if (this.ChangeItemsInListEvent != null)
            {
                Delegate[] InvocationList = this.ChangeItemsInListEvent.GetInvocationList();

                foreach (Delegate d in InvocationList)
                {
                    ChangeItemsInListDelegate d2 = d as ChangeItemsInListDelegate;

                    try
                    {
                        d2();
                    }
                    catch (Exception ex)
                    {
#if (DEBUG == FALSE)
                                            System.Diagnostics.Trace.TraceWarning("Обработчик \"{0}\" вызвал Exception: \"{1}\" и будет отписан от {2}, EventSender",
                                                d2.Method.ToString(), ex.Message, this.ChangeItemsInListEvent.Method.ToString());
                                            this.ChangeItemsInListEvent -= d2;


#else
                        if (/*NEED EXCEPTIONS*/ false)
                        {
                            throw ex.InnerException;
                        }
                        else
                        {
                            System.Diagnostics.Trace.TraceWarning("Обработчик \"{0}\" вызвал Exception: \"{1}\" и будет отписан от {2}, EventSender",
                                d2.Method.ToString(), ex.Message, this.ChangeItemsInListEvent.Method.ToString());
                            this.ChangeItemsInListEvent -= d2;
                        }
#endif
                    }
                }
            }
            #endregion
        }


        #region IList

        public void Add(T item)
        {
            MyList.Add(item);
            SendChangeItemsInListEvent();
        }

        public void Clear()
        {
            MyList.Clear();
            SendChangeItemsInListEvent();
        }

        public bool Contains(T item)
        {
            return MyList.Contains(item);
        }

        public int IndexOf(T item)
        {
            return MyList.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            MyList.Insert(index, item);
            SendChangeItemsInListEvent();
        }
                
        public bool IsReadOnly
        {
            get { return MyList.IsReadOnly; }
        }

        public bool Remove(T item)
        {
            bool temp=MyList.Remove(item);
            SendChangeItemsInListEvent();
            return temp;
        }

        public void RemoveAt(int index)
        {
            MyList.RemoveAt(index);
            SendChangeItemsInListEvent();
        }

        T IList<T>.this[int index]
        {
            get
            {
                return (T)MyList[index];
            }
            set
            {
                MyList[index] = value;
                SendChangeItemsInListEvent();
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            MyList.CopyTo(array,arrayIndex);
        }

        public int Count
        {
            get { return MyList.Count; }
        }

      

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return MyList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return MyList.GetEnumerator();
        }

        #endregion




        
    }
}
