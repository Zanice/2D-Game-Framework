using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class LinkedList<T> : IEnumerable<T> {
    Element head;
    Element tail;
    int size;

    public LinkedList() {
        head = tail = null;
        size = 0;
    }

    /// <summary>
    /// The number of elements contained in the list.
    /// </summary>
    public int Size {
        get {
            return size;
        }
    }

    /// <summary>
    /// Adds the specified object into the LinkedList. The added object may be repeated. Null values are not allowed for the object.
    /// </summary>
    /// <param name="item">The object to add to the list.</param>
    public void Add(T item) {
        if (null == item)
            throw new NullReferenceException("Cannot add null element to linked list.");

        Element newElement = new Element(item);

        if (size > 0) {
            tail.next = newElement;
            tail = newElement;
        }
        else
            head = tail = newElement;

        size++;
    }

    /// <summary>
    /// Attempts to add the specified object to the list, failing if the object already exists in the list. Null values are not allowed for the object.
    /// </summary>
    /// <param name="item">The object to add to the list.</param>
    /// <returns>True if the addition was successful, false otherwise.</returns>
    public bool AddUnique(T item) {
        if (Contains(item))
            return false;
        Add(item);
        return true;
    }

    /// <summary>
    /// Returns the object at the given index. Throws an IndexOutOfRangeException if the given index does not map to an object.
    /// </summary>
    /// <param name="index">The index to get the object from.</param>
    /// <returns>The object at the index.</returns>
    public T Get(int index) {
        if ((0 <= index) && (index < size)) {
            if (index == 0)
                return head.element;
            else {
                Element current = head;
                int currentIndex = 0;
                while (currentIndex < index) {
                    current = current.next;
                    currentIndex++;
                }
                return current.element;
            }
        }
        else
            throw new IndexOutOfRangeException();
    }

    /// <summary>
    /// Returns the head of the list, throwing a MissingMemberException if no such object exists.
    /// </summary>
    /// <returns>The head object of the list.</returns>
    public T GetFirst() {
        if (head == null)
            throw new MissingMemberException("There is no front element of this list.");

        return head.element;
    }

    /// <summary>
    /// Returns the tail of the list, throwing a MissingMemberException if no such object exists.
    /// </summary>
    /// <returns>The tail object of the list.</returns>
    public T GetLast() {
        if (tail == null)
            throw new MissingMemberException("There is no back element of this list.");

        return tail.element;
    }

    /// <summary>
    /// Returns true if the list contains the specified object.
    /// </summary>
    /// <param name="item">The object to search for.</param>
    /// <returns>True if the list contains the object, false otherwise.</returns>
    public bool Contains(T item) {
        Element current = head;
        while (current != null) {
            if (current.element.Equals(item))
                return true;
            current = current.next;
        }
        return false;
    }

    /// <summary>
    /// Attempts to remove the specified object from the list. If the object is not in the list, no action is taken.
    /// </summary>
    /// <param name="item">The object to remove from the list.</param>
    /// <returns>True if the object was in the list and has been successfully removed, false if the object was already no in the list.</returns>
    public bool Remove(T item) {
        if (0 == size)
            return false;

        if (head.element.Equals(item)) {
            head = head.next;
            size--;
            if (size == 0)
                tail = null;
            return true;
        }
        else if (size > 1) {
            Element prevElement = head;

            while (prevElement.next != null) {
                if (prevElement.next.element.Equals(item)) {
                    bool toReplaceTail = prevElement.next.Equals(tail);
                    prevElement.next = prevElement.next.next;
                    size--;
                    if (toReplaceTail)
                        tail = prevElement;
                    return true;
                }

                prevElement = prevElement.next;
            }
        }
        return false;
    }

    /// <summary>
    /// Removes all elements from the list, reducing the size to zero.
    /// </summary>
    public void Clear() {
        head = null;
        tail = null;
        size = 0;
    }

    /// <summary>
    /// Returns an array of the appropriate object type, representing the current list of objects.
    /// </summary>
    /// <returns></returns>
    public T[] AsArray() {
        T[] array = new T[size];
        int arrayIndex = 0;
        Element current = head;
        while (current != null) {
            array[arrayIndex++] = current.element;
            current = current.next;
        }

        return array;
    }

    /// <summary>
    /// Returns the string representation of this object.
    /// </summary>
    /// <returns>The string representation of this object.</returns>
    public override string ToString() {
        string output = "";
        Element current = head;
        while (current != null) {
            output += current.element.ToString() + ", ";
            current = current.next;
        }
        return output;
    }

    /// <summary>
    /// Defines how the object is iterated over.
    /// </summary>
    /// <returns>The IEnumerator as directed by the object's iterative behavior.</returns>
    public IEnumerator<T> GetEnumerator() {
        T[] elements = AsArray();

        for (int index = 0; index < elements.Length; index++)
            yield return elements[index];
    }

    /// <summary>
    /// Returns the object's enumerator for iteration.
    /// </summary>
    /// <returns>The object's enumerator.</returns>
    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }

    private class Element {
        public T element;
        public Element next;

        public Element(T element) {
            this.element = element;
            next = null;
        }
    }
}
