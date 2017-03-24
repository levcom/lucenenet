using System;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace Lucene.Net.Search
{
    /*
     * Licensed to the Apache Software Foundation (ASF) under one or more
     * contributor license agreements.  See the NOTICE file distributed with
     * this work for additional information regarding copyright ownership.
     * The ASF licenses this file to You under the Apache License, Version 2.0
     * (the "License"); you may not use this file except in compliance with
     * the License.  You may obtain a copy of the License at
     *
     *     http://www.apache.org/licenses/LICENSE-2.0
     *
     * Unless required by applicable law or agreed to in writing, software
     * distributed under the License is distributed on an "AS IS" BASIS,
     * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
     * See the License for the specific language governing permissions and
     * limitations under the License.
     */

    using AtomicReader = Lucene.Net.Index.AtomicReader;
    using BinaryDocValues = Lucene.Net.Index.BinaryDocValues;
    using IBits = Lucene.Net.Util.IBits;
    using BytesRef = Lucene.Net.Util.BytesRef;
    using NumericUtils = Lucene.Net.Util.NumericUtils;
    using RamUsageEstimator = Lucene.Net.Util.RamUsageEstimator;
    using SortedDocValues = Lucene.Net.Index.SortedDocValues;
    using SortedSetDocValues = Lucene.Net.Index.SortedSetDocValues;
    using Terms = Lucene.Net.Index.Terms;
    using TermsEnum = Lucene.Net.Index.TermsEnum;

    /// <summary>
    /// Expert: Maintains caches of term values.
    ///
    /// <p>Created: May 19, 2004 11:13:14 AM
    ///
    /// @since   lucene 1.4 </summary>
    /// <seealso cref=Lucene.Net.Util.FieldCacheSanityChecker</seealso>
    public interface IFieldCache
    {
        /// <summary>
        /// Checks the internal cache for an appropriate entry, and if none is found,
        ///  reads the terms in <code>field</code> and returns a bit set at the size of
        ///  <code>reader.maxDoc()</code>, with turned on bits for each docid that
        ///  does have a value for this field.
        /// </summary>
        IBits GetDocsWithField(AtomicReader reader, string field);

        /// <summary>
        /// Checks the internal cache for an appropriate entry, and if none is
        /// found, reads the terms in <code>field</code> as a single byte and returns an array
        /// of size <code>reader.maxDoc()</code> of the value each document
        /// has in the given field. </summary>
        /// <param name="reader">  Used to get field values. </param>
        /// <param name="field">   Which field contains the single byte values. </param>
        /// <param name="setDocsWithField">  If true then <seealso cref="#getDocsWithField"/> will
        ///        also be computed and stored in the FieldCache. </param>
        /// <returns> The values in the given field for each document. </returns>
        /// <exception cref="IOException">  If any error occurs. </exception>
        /// @deprecated (4.4) Index as a numeric field using <seealso cref="IntField"/> and then use <seealso cref="#getInts(AtomicReader, String, boolean)"/> instead.
        [Obsolete("(4.4) Index as a numeric field using IntField and then use GetInt32s(AtomicReader, string, bool) instead.")]
        FieldCache.Bytes GetBytes(AtomicReader reader, string field, bool setDocsWithField);

        /// <summary>
        /// Checks the internal cache for an appropriate entry, and if none is found,
        /// reads the terms in <code>field</code> as bytes and returns an array of
        /// size <code>reader.maxDoc()</code> of the value each document has in the
        /// given field. </summary>
        /// <param name="reader">  Used to get field values. </param>
        /// <param name="field">   Which field contains the bytes. </param>
        /// <param name="parser">  Computes byte for string values. </param>
        /// <param name="setDocsWithField">  If true then <seealso cref="#getDocsWithField"/> will
        ///        also be computed and stored in the FieldCache. </param>
        /// <returns> The values in the given field for each document. </returns>
        /// <exception cref="IOException">  If any error occurs. </exception>
        /// @deprecated (4.4) Index as a numeric field using <seealso cref="IntField"/> and then use <seealso cref="#getInts(AtomicReader, String, boolean)"/> instead.
        [Obsolete("(4.4) Index as a numeric field using IntField and then use GetInt32s(AtomicReader, string, bool) instead.")]
        FieldCache.Bytes GetBytes(AtomicReader reader, string field, FieldCache.IByteParser parser, bool setDocsWithField);

        /// <summary>
        /// Checks the internal cache for an appropriate entry, and if none is
        /// found, reads the terms in <code>field</code> as shorts and returns an array
        /// of size <code>reader.maxDoc()</code> of the value each document
        /// has in the given field. 
        /// <para/>
        /// NOTE: this was getShorts() in Lucene
        /// </summary>
        /// <param name="reader">  Used to get field values. </param>
        /// <param name="field">   Which field contains the shorts. </param>
        /// <param name="setDocsWithField">  If true then <seealso cref="#getDocsWithField"/> will
        ///        also be computed and stored in the FieldCache. </param>
        /// <returns> The values in the given field for each document. </returns>
        /// <exception cref="IOException">  If any error occurs. </exception>
        /// @deprecated (4.4) Index as a numeric field using <seealso cref="IntField"/> and then use <seealso cref="#getInts(AtomicReader, String, boolean)"/> instead.
        [Obsolete("(4.4) Index as a numeric field using Int32Field and then use GetInt32s(AtomicReader, string, bool) instead.")]
        FieldCache.Int16s GetInt16s(AtomicReader reader, string field, bool setDocsWithField);

        /// <summary>
        /// Checks the internal cache for an appropriate entry, and if none is found,
        /// reads the terms in <code>field</code> as shorts and returns an array of
        /// size <code>reader.maxDoc()</code> of the value each document has in the
        /// given field. 
        /// <para/>
        /// NOTE: this was getShorts() in Lucene
        /// </summary>
        /// <param name="reader">  Used to get field values. </param>
        /// <param name="field">   Which field contains the shorts. </param>
        /// <param name="parser">  Computes short for string values. </param>
        /// <param name="setDocsWithField">  If true then <seealso cref="#getDocsWithField"/> will
        ///        also be computed and stored in the FieldCache. </param>
        /// <returns> The values in the given field for each document. </returns>
        /// <exception cref="IOException">  If any error occurs. </exception>
        /// @deprecated (4.4) Index as a numeric field using <seealso cref="IntField"/> and then use <seealso cref="#getInts(AtomicReader, String, boolean)"/> instead.
        [Obsolete("(4.4) Index as a numeric field using Int32Field and then use GetInt32s(AtomicReader, string, bool) instead.")]
        FieldCache.Int16s GetInt16s(AtomicReader reader, string field, FieldCache.IInt16Parser parser, bool setDocsWithField);

        /// <summary>
        /// Returns an <seealso cref="FieldCache.Int32s"/> over the values found in documents in the given
        /// field.
        /// <para/>
        /// NOTE: this was getInts() in Lucene
        /// </summary>
        /// <seealso cref= #getInts(AtomicReader, String, IntParser, boolean) </seealso>
        FieldCache.Int32s GetInt32s(AtomicReader reader, string field, bool setDocsWithField);

        /// <summary>
        /// Returns an <seealso cref="FieldCache.Int32s"/> over the values found in documents in the given
        /// field. If the field was indexed as <seealso cref="NumericDocValuesField"/>, it simply
        /// uses <seealso cref="AtomicReader#getNumericDocValues(String)"/> to read the values.
        /// Otherwise, it checks the internal cache for an appropriate entry, and if
        /// none is found, reads the terms in <code>field</code> as ints and returns
        /// an array of size <code>reader.maxDoc()</code> of the value each document
        /// has in the given field.
        /// <para/>
        /// NOTE: this was getInts() in Lucene
        /// </summary>
        /// <param name="reader">
        ///          Used to get field values. </param>
        /// <param name="field">
        ///          Which field contains the longs. </param>
        /// <param name="parser">
        ///          Computes int for string values. May be {@code null} if the
        ///          requested field was indexed as <seealso cref="NumericDocValuesField"/> or
        ///          <seealso cref="IntField"/>. </param>
        /// <param name="setDocsWithField">
        ///          If true then <seealso cref="#getDocsWithField"/> will also be computed and
        ///          stored in the FieldCache. </param>
        /// <returns> The values in the given field for each document. </returns>
        /// <exception cref="IOException">
        ///           If any error occurs. </exception>
        FieldCache.Int32s GetInt32s(AtomicReader reader, string field, FieldCache.IInt32Parser parser, bool setDocsWithField);

        /// <summary>
        /// Returns a <seealso cref="Floats"/> over the values found in documents in the given
        /// field.
        /// <para/>
        /// NOTE: this was getFloats() in Lucene
        /// </summary>
        /// <seealso cref= #getFloats(AtomicReader, String, FloatParser, boolean) </seealso>
        FieldCache.Singles GetSingles(AtomicReader reader, string field, bool setDocsWithField);

        /// <summary>
        /// Returns a <seealso cref="Floats"/> over the values found in documents in the given
        /// field. If the field was indexed as <seealso cref="NumericDocValuesField"/>, it simply
        /// uses <seealso cref="AtomicReader#getNumericDocValues(String)"/> to read the values.
        /// Otherwise, it checks the internal cache for an appropriate entry, and if
        /// none is found, reads the terms in <code>field</code> as floats and returns
        /// an array of size <code>reader.maxDoc()</code> of the value each document
        /// has in the given field.
        /// <para/>
        /// NOTE: this was getFloats() in Lucene
        /// </summary>
        /// <param name="reader">
        ///          Used to get field values. </param>
        /// <param name="field">
        ///          Which field contains the floats. </param>
        /// <param name="parser">
        ///          Computes float for string values. May be {@code null} if the
        ///          requested field was indexed as <seealso cref="NumericDocValuesField"/> or
        ///          <seealso cref="FloatField"/>. </param>
        /// <param name="setDocsWithField">
        ///          If true then <seealso cref="#getDocsWithField"/> will also be computed and
        ///          stored in the FieldCache. </param>
        /// <returns> The values in the given field for each document. </returns>
        /// <exception cref="IOException">
        ///           If any error occurs. </exception>
        FieldCache.Singles GetSingles(AtomicReader reader, string field, FieldCache.ISingleParser parser, bool setDocsWithField);

        /// <summary>
        /// Returns a <seealso cref="Longs"/> over the values found in documents in the given
        /// field.
        /// <para/>
        /// NOTE: this was getLongs() in Lucene
        /// </summary>
        /// <seealso cref= #getLongs(AtomicReader, String, LongParser, boolean) </seealso>
        FieldCache.Int64s GetInt64s(AtomicReader reader, string field, bool setDocsWithField);

        /// <summary>
        /// Returns a <seealso cref="Longs"/> over the values found in documents in the given
        /// field. If the field was indexed as <seealso cref="NumericDocValuesField"/>, it simply
        /// uses <seealso cref="AtomicReader#getNumericDocValues(String)"/> to read the values.
        /// Otherwise, it checks the internal cache for an appropriate entry, and if
        /// none is found, reads the terms in <code>field</code> as longs and returns
        /// an array of size <code>reader.maxDoc()</code> of the value each document
        /// has in the given field.
        /// <para/>
        /// NOTE: this was getLongs() in Lucene
        /// </summary>
        /// <param name="reader">
        ///          Used to get field values. </param>
        /// <param name="field">
        ///          Which field contains the longs. </param>
        /// <param name="parser">
        ///          Computes long for string values. May be {@code null} if the
        ///          requested field was indexed as <seealso cref="NumericDocValuesField"/> or
        ///          <seealso cref="LongField"/>. </param>
        /// <param name="setDocsWithField">
        ///          If true then <seealso cref="#getDocsWithField"/> will also be computed and
        ///          stored in the FieldCache. </param>
        /// <returns> The values in the given field for each document. </returns>
        /// <exception cref="IOException">
        ///           If any error occurs. </exception>
        FieldCache.Int64s GetInt64s(AtomicReader reader, string field, FieldCache.IInt64Parser parser, bool setDocsWithField);

        /// <summary>
        /// Returns a <seealso cref="Doubles"/> over the values found in documents in the given
        /// field.
        /// </summary>
        /// <seealso cref= #getDoubles(AtomicReader, String, DoubleParser, boolean) </seealso>
        FieldCache.Doubles GetDoubles(AtomicReader reader, string field, bool setDocsWithField);

        /// <summary>
        /// Returns a <seealso cref="Doubles"/> over the values found in documents in the given
        /// field. If the field was indexed as <seealso cref="NumericDocValuesField"/>, it simply
        /// uses <seealso cref="AtomicReader#getNumericDocValues(String)"/> to read the values.
        /// Otherwise, it checks the internal cache for an appropriate entry, and if
        /// none is found, reads the terms in <code>field</code> as doubles and returns
        /// an array of size <code>reader.maxDoc()</code> of the value each document
        /// has in the given field.
        /// </summary>
        /// <param name="reader">
        ///          Used to get field values. </param>
        /// <param name="field">
        ///          Which field contains the longs. </param>
        /// <param name="parser">
        ///          Computes double for string values. May be {@code null} if the
        ///          requested field was indexed as <seealso cref="NumericDocValuesField"/> or
        ///          <seealso cref="DoubleField"/>. </param>
        /// <param name="setDocsWithField">
        ///          If true then <seealso cref="#getDocsWithField"/> will also be computed and
        ///          stored in the FieldCache. </param>
        /// <returns> The values in the given field for each document. </returns>
        /// <exception cref="IOException">
        ///           If any error occurs. </exception>
        FieldCache.Doubles GetDoubles(AtomicReader reader, string field, FieldCache.IDoubleParser parser, bool setDocsWithField);

        /// <summary>
        /// Checks the internal cache for an appropriate entry, and if none
        /// is found, reads the term values in <code>field</code>
        /// and returns a <seealso cref="BinaryDocValues"/> instance, providing a
        /// method to retrieve the term (as a BytesRef) per document. </summary>
        /// <param name="reader">  Used to get field values. </param>
        /// <param name="field">   Which field contains the strings. </param>
        /// <param name="setDocsWithField">  If true then <seealso cref="#getDocsWithField"/> will
        ///        also be computed and stored in the FieldCache. </param>
        /// <returns> The values in the given field for each document. </returns>
        /// <exception cref="IOException">  If any error occurs. </exception>
        BinaryDocValues GetTerms(AtomicReader reader, string field, bool setDocsWithField);

        /// <summary>
        /// Expert: just like <seealso cref="#getTerms(AtomicReader,String,boolean)"/>,
        ///  but you can specify whether more RAM should be consumed in exchange for
        ///  faster lookups (default is "true").  Note that the
        ///  first call for a given reader and field "wins",
        ///  subsequent calls will share the same cache entry.
        /// </summary>
        BinaryDocValues GetTerms(AtomicReader reader, string field, bool setDocsWithField, float acceptableOverheadRatio);

        /// <summary>
        /// Checks the internal cache for an appropriate entry, and if none
        /// is found, reads the term values in <code>field</code>
        /// and returns a <seealso cref="SortedDocValues"/> instance,
        /// providing methods to retrieve sort ordinals and terms
        /// (as a ByteRef) per document. </summary>
        /// <param name="reader">  Used to get field values. </param>
        /// <param name="field">   Which field contains the strings. </param>
        /// <returns> The values in the given field for each document. </returns>
        /// <exception cref="IOException">  If any error occurs. </exception>
        SortedDocValues GetTermsIndex(AtomicReader reader, string field);

        /// <summary>
        /// Expert: just like {@link
        ///  #getTermsIndex(AtomicReader,String)}, but you can specify
        ///  whether more RAM should be consumed in exchange for
        ///  faster lookups (default is "true").  Note that the
        ///  first call for a given reader and field "wins",
        ///  subsequent calls will share the same cache entry.
        /// </summary>
        SortedDocValues GetTermsIndex(AtomicReader reader, string field, float acceptableOverheadRatio);

        /// <summary>
        /// Checks the internal cache for an appropriate entry, and if none is found, reads the term values
        /// in <code>field</code> and returns a <seealso cref="DocTermOrds"/> instance, providing a method to retrieve
        /// the terms (as ords) per document.
        /// </summary>
        /// <param name="reader">  Used to build a <seealso cref="DocTermOrds"/> instance </param>
        /// <param name="field">   Which field contains the strings. </param>
        /// <returns> a <seealso cref="DocTermOrds"/> instance </returns>
        /// <exception cref="IOException">  If any error occurs. </exception>
        SortedSetDocValues GetDocTermOrds(AtomicReader reader, string field);

        /// <summary>
        /// EXPERT: A unique Identifier/Description for each item in the FieldCache.
        /// Can be useful for logging/debugging.
        /// @lucene.experimental
        /// </summary>

        /// <summary>
        /// EXPERT: Generates an array of CacheEntry objects representing all items
        /// currently in the FieldCache.
        /// <p>
        /// NOTE: These CacheEntry objects maintain a strong reference to the
        /// Cached Values.  Maintaining references to a CacheEntry the AtomicIndexReader
        /// associated with it has garbage collected will prevent the Value itself
        /// from being garbage collected when the Cache drops the WeakReference.
        /// </p>
        /// @lucene.experimental
        /// </summary>
        FieldCache.CacheEntry[] GetCacheEntries();

        /// <summary>
        /// <p>
        /// EXPERT: Instructs the FieldCache to forcibly expunge all entries
        /// from the underlying caches.  this is intended only to be used for
        /// test methods as a way to ensure a known base state of the Cache
        /// (with out needing to rely on GC to free WeakReferences).
        /// It should not be relied on for "Cache maintenance" in general
        /// application code.
        /// </p>
        /// @lucene.experimental
        /// </summary>
        void PurgeAllCaches();

        /// <summary>
        /// Expert: drops all cache entries associated with this
        /// reader <seealso cref="IndexReader#getCoreCacheKey"/>.  NOTE: this cache key must
        /// precisely match the reader that the cache entry is
        /// keyed on. If you pass a top-level reader, it usually
        /// will have no effect as Lucene now caches at the segment
        /// reader level.
        /// </summary>
        void PurgeByCacheKey(object coreCacheKey);

        /// <summary>
        /// If non-null, FieldCacheImpl will warn whenever
        /// entries are created that are not sane according to
        /// <seealso cref="Lucene.Net.Util.FieldCacheSanityChecker"/>.
        /// </summary>
        TextWriter InfoStream { set; get; }
    }

    // LUCENENET TODO: Copy documentation from Lucene

    public static class FieldCache 
    {
        public abstract class Bytes
        {
            public abstract byte Get(int docID);

            public static readonly Bytes EMPTY = new EmptyBytes();

            public sealed class EmptyBytes : Bytes
            {
                public override byte Get(int docID)
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// NOTE: This was Shorts in Lucene
        /// </summary>
        public abstract class Int16s
        {
            public abstract short Get(int docID);

            public static readonly Int16s EMPTY = new EmptyInt16s();

            /// <summary>
            /// NOTE: This was EmptyShorts in Lucene
            /// </summary>
            public sealed class EmptyInt16s : Int16s
            {
                public override short Get(int docID)
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// NOTE: This was Ints in Lucene
        /// </summary>
        public abstract class Int32s
        {
            public abstract int Get(int docID);

            public static readonly Int32s EMPTY = new EmptyInt32s();

            /// <summary>
            /// NOTE: This was EmptyInts in Lucene
            /// </summary>
            public sealed class EmptyInt32s : Int32s
            {
                public override int Get(int docID)
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// NOTE: This was Longs in Lucene
        /// </summary>
        public abstract class Int64s
        {
            public abstract long Get(int docID);

            public static readonly Int64s EMPTY = new EmptyInt64s();

            /// <summary>
            /// NOTE: This was EmptyLongs in Lucene
            /// </summary>
            public sealed class EmptyInt64s : Int64s
            {
                public override long Get(int docID)
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// NOTE: This was Floats in Lucene
        /// </summary>
        public abstract class Singles
        {
            public abstract float Get(int docID);

            public static readonly Singles EMPTY = new EmptySingles();

            /// <summary>
            /// NOTE: This was EmptySingles in Lucene
            /// </summary>
            public sealed class EmptySingles : Singles
            {
                public override float Get(int docID)
                {
                    return 0;
                }
            }
        }

        public abstract class Doubles
        {
            public abstract double Get(int docID);

            public static readonly Doubles EMPTY = new EmptyDoubles();

            public sealed class EmptyDoubles : Doubles
            {
                public override double Get(int docID)
                {
                    return 0;
                }
            }
        }

        public sealed class CreationPlaceholder
        {
            internal object Value { get; set; }
        }

        public interface IParser
        {
            TermsEnum TermsEnum(Terms terms);
        }

        public interface IByteParser : IParser
        {
            byte ParseByte(BytesRef term);
        }

        /// <summary>
        /// NOTE: This was ShortParser in Lucene
        /// </summary>
        public interface IInt16Parser : IParser
        {
            /// <summary>
            /// NOTE: This was parseShort() in Lucene
            /// </summary>
            short ParseInt16(BytesRef term);
        }

        /// <summary>
        /// NOTE: This was IntParser in Lucene
        /// </summary>
        public interface IInt32Parser : IParser
        {
            /// <summary>
            /// NOTE: This was parseInt() in Lucene
            /// </summary>
            int ParseInt32(BytesRef term);
        }

        /// <summary>
        /// NOTE: This was FloatParser in Lucene
        /// </summary>
        public interface ISingleParser : IParser
        {
            /// <summary>
            /// NOTE: This was parseFloat() in Lucene
            /// </summary>
            float ParseSingle(BytesRef term);
        }

        /// <summary>
        /// NOTE: This was LongParser in Lucene
        /// </summary>
        public interface IInt64Parser : IParser
        {
            /// <summary>
            /// NOTE: This was parseLong() in Lucene
            /// </summary>
            long ParseInt64(BytesRef term);
        }

        public interface IDoubleParser : IParser
        {
            double ParseDouble(BytesRef term);
        }

        public static IFieldCache DEFAULT = new FieldCacheImpl();

        public static readonly IByteParser DEFAULT_BYTE_PARSER = new AnonymousByteParser();

        private sealed class AnonymousByteParser : IByteParser
        {
            public byte ParseByte(BytesRef term)
            {
                // TODO: would be far better to directly parse from
                // UTF8 bytes... but really users should use
                // IntField, instead, which already decodes
                // directly from byte[]
                return (byte)sbyte.Parse(term.Utf8ToString(), CultureInfo.InvariantCulture);
            }

            public override string ToString()
            {
                return typeof(IFieldCache).FullName + ".DEFAULT_BYTE_PARSER";
            }

            public TermsEnum TermsEnum(Terms terms)
            {
                return terms.GetIterator(null);
            }
        }

        /// <summary>
        /// NOTE: This was DEFAULT_SHORT_PARSER in Lucene
        /// </summary>
        public static readonly IInt16Parser DEFAULT_INT16_PARSER = new AnonymousInt16Parser();


        private sealed class AnonymousInt16Parser : IInt16Parser
        {
            /// <summary>
            /// NOTE: This was parseShort() in Lucene
            /// </summary>
            public short ParseInt16(BytesRef term)
            {
                // TODO: would be far better to directly parse from
                // UTF8 bytes... but really users should use
                // IntField, instead, which already decodes
                // directly from byte[]
                return short.Parse(term.Utf8ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture);
            }

            public override string ToString()
            {
                return typeof(IFieldCache).FullName + ".DEFAULT_INT16_PARSER";
            }

            public TermsEnum TermsEnum(Terms terms)
            {
                return terms.GetIterator(null);
            }
        }

        /// <summary>
        /// NOTE: This was DEFAULT_INT_PARSER in Lucene
        /// </summary>
        public static readonly IInt32Parser DEFAULT_INT32_PARSER = new AnonymousInt32Parser();

        private sealed class AnonymousInt32Parser : IInt32Parser
        {
            /// <summary>
            /// NOTE: This was parseInt() in Lucene
            /// </summary>
            public int ParseInt32(BytesRef term)
            {
                // TODO: would be far better to directly parse from
                // UTF8 bytes... but really users should use
                // IntField, instead, which already decodes
                // directly from byte[]
                return int.Parse(term.Utf8ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture);
            }

            public TermsEnum TermsEnum(Terms terms)
            {
                return terms.GetIterator(null);
            }

            public override string ToString()
            {
                return typeof(IFieldCache).FullName + ".DEFAULT_INT32_PARSER";
            }
        }

        /// <summary>
        /// NOTE: This was DEFAULT_FLOAT_PARSER in Lucene
        /// </summary>
        public static readonly ISingleParser DEFAULT_SINGLE_PARSER = new AnonymousSingleParser();

        private sealed class AnonymousSingleParser : ISingleParser
        {
            /// <summary>
            /// NOTE: This was parseFloat() in Lucene
            /// </summary>
            public float ParseSingle(BytesRef term)
            {
                // TODO: would be far better to directly parse from
                // UTF8 bytes... but really users should use
                // FloatField, instead, which already decodes
                // directly from byte[]

                // LUCENENET: We parse to double first and then cast to float, which allows us to parse 
                // double.MaxValue.ToString("R") (resulting in Infinity). This is how it worked in Java
                // and the TestFieldCache.TestInfoStream() test depends on this behavior to pass.

                // We also need to use the same logic as DEFAULT_DOUBLE_PARSER to ensure we have signed zero
                // support, so just call it directly rather than duplicating the logic here.
                return (float)DEFAULT_DOUBLE_PARSER.ParseDouble(term);
            }

            public TermsEnum TermsEnum(Terms terms)
            {
                return terms.GetIterator(null);
            }

            public override string ToString()
            {
                return typeof(IFieldCache).FullName + ".DEFAULT_SINGLE_PARSER";
            }
        }

        /// <summary>
        /// NOTE: This was DEFAULT_LONG_PARSER in Lucene
        /// </summary>
        public static readonly IInt64Parser DEFAULT_INT64_PARSER = new AnonymousInt64Parser();

        private sealed class AnonymousInt64Parser : IInt64Parser
        {
            /// <summary>
            /// NOTE: This was parseLong() in Lucene
            /// </summary>
            public long ParseInt64(BytesRef term)
            {
                // TODO: would be far better to directly parse from
                // UTF8 bytes... but really users should use
                // LongField, instead, which already decodes
                // directly from byte[]
                return long.Parse(term.Utf8ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture);
            }

            public TermsEnum TermsEnum(Terms terms)
            {
                return terms.GetIterator(null);
            }

            public override string ToString()
            {
                return typeof(IFieldCache).FullName + ".DEFAULT_INT64_PARSER";
            }
        }

        public static readonly IDoubleParser DEFAULT_DOUBLE_PARSER = new AnonymousDoubleParser();

        private sealed class AnonymousDoubleParser : IDoubleParser
        {
            public double ParseDouble(BytesRef term)
            {
                // TODO: would be far better to directly parse from
                // UTF8 bytes... but really users should use
                // DoubleField, instead, which already decodes
                // directly from byte[]
                string text = term.Utf8ToString();
                double value = double.Parse(text, NumberStyles.Float, CultureInfo.InvariantCulture);

                // LUCENENET specific special case - check whether a negative
                // zero was passed in and, if so, convert the sign. Unfotunately, double.Parse()
                // doesn't take care of this for us.
                if (value == 0 && text.TrimStart().StartsWith("-", StringComparison.Ordinal))
                {
                    value = -0d; // Hard-coding the value in case double.Parse() works right someday (which would break if we did value * -1)
                }

                return value;
            }

            public TermsEnum TermsEnum(Terms terms)
            {
                return terms.GetIterator(null);
            }

            public override string ToString()
            {
                return typeof(IFieldCache).FullName + ".DEFAULT_DOUBLE_PARSER";
            }
        }

        /// <summary>
        /// NOTE: This was NUMERIC_UTILS_INT_PARSER in Lucene
        /// </summary>
        public static readonly IInt32Parser NUMERIC_UTILS_INT32_PARSER = new AnonymousNumericUtilsInt32Parser();

        private sealed class AnonymousNumericUtilsInt32Parser : IInt32Parser
        {
            /// <summary>
            /// NOTE: This was parseInt() in Lucene
            /// </summary>
            public int ParseInt32(BytesRef term)
            {
                return NumericUtils.PrefixCodedToInt32(term);
            }

            public TermsEnum TermsEnum(Terms terms)
            {
                return NumericUtils.FilterPrefixCodedInt32s(terms.GetIterator(null));
            }

            public override string ToString()
            {
                return typeof(IFieldCache).FullName + ".NUMERIC_UTILS_INT32_PARSER";
            }
        }

        /// <summary>
        /// NOTE: This was NUMERIC_UTILS_FLOAT_PARSER in Lucene
        /// </summary>
        public static readonly ISingleParser NUMERIC_UTILS_SINGLE_PARSER = new AnonymousNumericUtilsSingleParser();

        private sealed class AnonymousNumericUtilsSingleParser : ISingleParser
        {
            /// <summary>
            /// NOTE: This was parseFloat() in Lucene
            /// </summary>
            public float ParseSingle(BytesRef term)
            {
                return NumericUtils.SortableInt32ToSingle(NumericUtils.PrefixCodedToInt32(term));
            }

            public override string ToString()
            {
                return typeof(IFieldCache).FullName + ".NUMERIC_UTILS_SINGLE_PARSER";
            }

            public TermsEnum TermsEnum(Terms terms)
            {
                return NumericUtils.FilterPrefixCodedInt32s(terms.GetIterator(null));
            }
        }

        /// <summary>
        /// NOTE: This was NUMERIC_UTILS_LONG_PARSER in Lucene
        /// </summary>
        public static readonly IInt64Parser NUMERIC_UTILS_INT64_PARSER = new AnonymousNumericUtilsInt64Parser();

        private sealed class AnonymousNumericUtilsInt64Parser : IInt64Parser
        {
            /// <summary>
            /// NOTE: This was parseLong() in Lucene
            /// </summary>
            public long ParseInt64(BytesRef term)
            {
                return NumericUtils.PrefixCodedToInt64(term);
            }

            public override string ToString()
            {
                return typeof(IFieldCache).FullName + ".NUMERIC_UTILS_INT64_PARSER";
            }

            public TermsEnum TermsEnum(Terms terms)
            {
                return NumericUtils.FilterPrefixCodedInt64s(terms.GetIterator(null));
            }
        }

        public static readonly IDoubleParser NUMERIC_UTILS_DOUBLE_PARSER = new AnonymousNumericUtilsDoubleParser();

        private sealed class AnonymousNumericUtilsDoubleParser : IDoubleParser
        {
            public double ParseDouble(BytesRef term)
            {
                return NumericUtils.SortableInt64ToDouble(NumericUtils.PrefixCodedToInt64(term));
            }

            public override string ToString()
            {
                return typeof(IFieldCache).FullName + ".NUMERIC_UTILS_DOUBLE_PARSER";
            }

            public TermsEnum TermsEnum(Terms terms)
            {
                return NumericUtils.FilterPrefixCodedInt64s(terms.GetIterator(null));
            }
        }

        // .NET Port: skipping down to about line 681 of java version. The actual interface methods of FieldCache are in IFieldCache below.
        public sealed class CacheEntry
        {
            private readonly object readerKey;
            private readonly string fieldName;
            private readonly Type cacheType;
            private readonly object custom;
            private readonly object value;
            private string size;

            public CacheEntry(object readerKey, string fieldName,
                      Type cacheType,
                      object custom,
                      object value)
            {
                this.readerKey = readerKey;
                this.fieldName = fieldName;
                this.cacheType = cacheType;
                this.custom = custom;
                this.value = value;
            }

            public object ReaderKey
            {
                get { return readerKey; }
            }

            public string FieldName
            {
                get { return fieldName; }
            }

            public Type CacheType
            {
                get { return cacheType; }
            }

            public object Custom
            {
                get { return custom; }
            }

            public object Value
            {
                get { return value; }
            }

            public void EstimateSize()
            {
                long bytesUsed = RamUsageEstimator.SizeOf(Value);
                size = RamUsageEstimator.HumanReadableUnits(bytesUsed);
            }

            public string EstimatedSize
            {
                get { return size; }
            }

            public override string ToString()
            {
                StringBuilder b = new StringBuilder();
                b.Append("'").Append(ReaderKey).Append("'=>");
                b.Append("'").Append(FieldName).Append("',");
                b.Append(CacheType).Append(",").Append(Custom);
                b.Append("=>").Append(Value.GetType().FullName).Append("#");
                b.Append(RuntimeHelpers.GetHashCode(Value));

                String s = EstimatedSize;
                if (null != s)
                {
                    b.Append(" (size =~ ").Append(s).Append(')');
                }

                return b.ToString();
            }
        }
    }
    
//  // LUCENENET NOTE: refactoring because of enum nonsense
//  public static readonly FieldCache DEFAULT = new FieldCacheImpl();

//  private class FieldCache_ByteParserAnonymousInnerClassHelper : FieldCache_ByteParser
//  {
//      public FieldCache_ByteParserAnonymousInnerClassHelper()
//      {
//      }

//      public virtual sbyte ParseByte(BytesRef term)
//      {
//      // TODO: would be far better to directly parse from
//      // UTF8 bytes... but really users should use
//      // IntField, instead, which already decodes
//      // directly from byte[]
//      return (sbyte)Convert.ToByte(term.Utf8ToString());
//      }
//    public override string ToString()
//    {
//      return typeof(FieldCache).Name + ".DEFAULT_BYTE_PARSER";
//    }
//    public virtual TermsEnum TermsEnum(Terms terms)
//    {
//      return terms.Iterator(null);
//    }
//  }

//  private class FieldCache_ShortParserAnonymousInnerClassHelper : FieldCache_ShortParser
//  {
//      public FieldCache_ShortParserAnonymousInnerClassHelper()
//      {
//      }

//      public virtual short ParseShort(BytesRef term)
//      {
//      // TODO: would be far better to directly parse from
//      // UTF8 bytes... but really users should use
//      // IntField, instead, which already decodes
//      // directly from byte[]
//      return Convert.ToInt16(term.Utf8ToString());
//      }
//    public override string ToString()
//    {
//      return typeof(FieldCache).Name + ".DEFAULT_SHORT_PARSER";
//    }

//    public virtual TermsEnum TermsEnum(Terms terms)
//    {
//      return terms.Iterator(null);
//    }
//  }

//  private class FieldCache_IntParserAnonymousInnerClassHelper : FieldCache_IntParser
//  {
//      public FieldCache_IntParserAnonymousInnerClassHelper()
//      {
//      }

//      public virtual int ParseInt(BytesRef term)
//      {
//      // TODO: would be far better to directly parse from
//      // UTF8 bytes... but really users should use
//      // IntField, instead, which already decodes
//      // directly from byte[]
//      return Convert.ToInt32(term.Utf8ToString());
//      }

//    public virtual TermsEnum TermsEnum(Terms terms)
//    {
//      return terms.Iterator(null);
//    }

//    public override string ToString()
//    {
//      return typeof(FieldCache).Name + ".DEFAULT_INT_PARSER";
//    }
//  }

//  private class FieldCache_FloatParserAnonymousInnerClassHelper : FieldCache_FloatParser
//  {
//      public FieldCache_FloatParserAnonymousInnerClassHelper()
//      {
//      }

//      public virtual float ParseFloat(BytesRef term)
//      {
//      // TODO: would be far better to directly parse from
//      // UTF8 bytes... but really users should use
//      // FloatField, instead, which already decodes
//      // directly from byte[]
//      return Convert.ToSingle(term.Utf8ToString());
//      }

//    public virtual TermsEnum TermsEnum(Terms terms)
//    {
//      return terms.Iterator(null);
//    }

//    public override string ToString()
//    {
//      return typeof(FieldCache).Name + ".DEFAULT_FLOAT_PARSER";
//    }
//  }

//  private class FieldCache_LongParserAnonymousInnerClassHelper : FieldCache_LongParser
//  {
//      public FieldCache_LongParserAnonymousInnerClassHelper()
//      {
//      }

//      public virtual long ParseLong(BytesRef term)
//      {
//      // TODO: would be far better to directly parse from
//      // UTF8 bytes... but really users should use
//      // LongField, instead, which already decodes
//      // directly from byte[]
//      return Convert.ToInt64(term.Utf8ToString());
//      }

//    public virtual TermsEnum TermsEnum(Terms terms)
//    {
//      return terms.Iterator(null);
//    }

//    public override string ToString()
//    {
//      return typeof(FieldCache).Name + ".DEFAULT_LONG_PARSER";
//    }
//  }

//  private class FieldCache_DoubleParserAnonymousInnerClassHelper : FieldCache_DoubleParser
//  {
//      public FieldCache_DoubleParserAnonymousInnerClassHelper()
//      {
//      }

//      public virtual double ParseDouble(BytesRef term)
//      {
//      // TODO: would be far better to directly parse from
//      // UTF8 bytes... but really users should use
//      // DoubleField, instead, which already decodes
//      // directly from byte[]
//      return Convert.ToDouble(term.Utf8ToString());
//      }

//    public virtual TermsEnum TermsEnum(Terms terms)
//    {
//      return terms.Iterator(null);
//    }

//    public override string ToString()
//    {
//      return typeof(FieldCache).Name + ".DEFAULT_DOUBLE_PARSER";
//    }
//  }

//  private class FieldCache_IntParserAnonymousInnerClassHelper2 : FieldCache_IntParser
//  {
//      public FieldCache_IntParserAnonymousInnerClassHelper2()
//      {
//      }

//      public override int ParseInt(BytesRef term)
//      {
//        return NumericUtils.PrefixCodedToInt(term);
//      }

//      public override TermsEnum TermsEnum(Terms terms)
//      {
//        return NumericUtils.FilterPrefixCodedInts(terms.Iterator(null));
//      }

//      public override string ToString()
//      {
//        return typeof(FieldCache).Name + ".NUMERIC_UTILS_INT_PARSER";
//      }
//  }

//  private class FieldCache_FloatParserAnonymousInnerClassHelper2 : FieldCache_FloatParser
//  {
//      public FieldCache_FloatParserAnonymousInnerClassHelper2()
//      {
//      }

//      public override float ParseFloat(BytesRef term)
//      {
//        return NumericUtils.SortableIntToFloat(NumericUtils.PrefixCodedToInt(term));
//      }
//      public override string ToString()
//      {
//        return typeof(FieldCache).Name + ".NUMERIC_UTILS_FLOAT_PARSER";
//      }

//      public override TermsEnum TermsEnum(Terms terms)
//      {
//        return NumericUtils.FilterPrefixCodedInts(terms.Iterator(null));
//      }
//  }

//  private class FieldCache_LongParserAnonymousInnerClassHelper2 : FieldCache_LongParser
//  {
//      public FieldCache_LongParserAnonymousInnerClassHelper2()
//      {
//      }

//      public override long ParseLong(BytesRef term)
//      {
//        return NumericUtils.PrefixCodedToLong(term);
//      }
//      public override string ToString()
//      {
//        return typeof(FieldCache).Name + ".NUMERIC_UTILS_LONG_PARSER";
//      }

//      public override TermsEnum TermsEnum(Terms terms)
//      {
//        return NumericUtils.FilterPrefixCodedLongs(terms.Iterator(null));
//      }
//  }

//  private class FieldCache_DoubleParserAnonymousInnerClassHelper2 : FieldCache_DoubleParser
//  {
//      public FieldCache_DoubleParserAnonymousInnerClassHelper2()
//      {
//      }

//      public override double ParseDouble(BytesRef term)
//      {
//        return NumericUtils.SortableLongToDouble(NumericUtils.PrefixCodedToLong(term));
//      }
//      public override string ToString()
//      {
//        return typeof(FieldCache).Name + ".NUMERIC_UTILS_DOUBLE_PARSER";
//      }

//      public override TermsEnum TermsEnum(Terms terms)
//      {
//        return NumericUtils.FilterPrefixCodedLongs(terms.Iterator(null));
//      }
//  }
//}

//  public abstract class FieldCache_Bytes
//  {
//    /// <summary>
//    /// Return a single Byte representation of this field's value. </summary>
//    public abstract sbyte Get(int docID);

//    /// <summary>
//    /// Zero value for every document </summary>
//    public static readonly FieldCache_Bytes EMPTY = new FieldCache_BytesAnonymousInnerClassHelper();

//    private class FieldCache_BytesAnonymousInnerClassHelper : FieldCache_Bytes
//    {
//        public FieldCache_BytesAnonymousInnerClassHelper()
//        {
//        }

//      public override sbyte Get(int docID)
//      {
//        return 0;
//      }
//    }
//  }

//  public abstract class FieldCache_Shorts
//  {
//    /// <summary>
//    /// Return a short representation of this field's value. </summary>
//    public abstract short Get(int docID);

//    /// <summary>
//    /// Zero value for every document </summary>
//    public static readonly FieldCache_Shorts EMPTY = new FieldCache_ShortsAnonymousInnerClassHelper();

//    private class FieldCache_ShortsAnonymousInnerClassHelper : FieldCache_Shorts
//    {
//        public FieldCache_ShortsAnonymousInnerClassHelper()
//        {
//        }

//      public override short Get(int docID)
//      {
//        return 0;
//      }
//    }
//  }

//  public abstract class FieldCache_Ints
//  {
//    /// <summary>
//    /// Return an integer representation of this field's value. </summary>
//    public abstract int Get(int docID);

//    /// <summary>
//    /// Zero value for every document </summary>
//    public static readonly FieldCache_Ints EMPTY = new FieldCache_IntsAnonymousInnerClassHelper();

//    private class FieldCache_IntsAnonymousInnerClassHelper : FieldCache_Ints
//    {
//        public FieldCache_IntsAnonymousInnerClassHelper()
//        {
//        }

//      public override int Get(int docID)
//      {
//        return 0;
//      }
//    }
//  }

//  public abstract class FieldCache_Longs
//  {
//    /// <summary>
//    /// Return an long representation of this field's value. </summary>
//    public abstract long Get(int docID);

//    /// <summary>
//    /// Zero value for every document </summary>
//    public static readonly FieldCache_Longs EMPTY = new FieldCache_LongsAnonymousInnerClassHelper();

//    private class FieldCache_LongsAnonymousInnerClassHelper : FieldCache_Longs
//    {
//        public FieldCache_LongsAnonymousInnerClassHelper()
//        {
//        }

//      public override long Get(int docID)
//      {
//        return 0;
//      }
//    }
//  }

//  public abstract class FieldCache_Floats
//  {
//    /// <summary>
//    /// Return an float representation of this field's value. </summary>
//    public abstract float Get(int docID);

//    /// <summary>
//    /// Zero value for every document </summary>
//    public static readonly FieldCache_Floats EMPTY = new FieldCache_FloatsAnonymousInnerClassHelper();

//    private class FieldCache_FloatsAnonymousInnerClassHelper : FieldCache_Floats
//    {
//        public FieldCache_FloatsAnonymousInnerClassHelper()
//        {
//        }

//      public override float Get(int docID)
//      {
//        return 0;
//      }
//    }
//  }

//  public abstract class FieldCache_Doubles
//  {
//    /// <summary>
//    /// Return an double representation of this field's value. </summary>
//    public abstract double Get(int docID);

//    /// <summary>
//    /// Zero value for every document </summary>
//    public static readonly FieldCache_Doubles EMPTY = new FieldCache_DoublesAnonymousInnerClassHelper();

//    private class FieldCache_DoublesAnonymousInnerClassHelper : FieldCache_Doubles
//    {
//        public FieldCache_DoublesAnonymousInnerClassHelper()
//        {
//        }

//      public override double Get(int docID)
//      {
//        return 0;
//      }
//    }
//  }

//  public sealed class FieldCache_CreationPlaceholder
//  {
//    internal object Value;
//  }

//  public interface FieldCache_Parser
//  {
//    /// <summary>
//    /// Pulls a <seealso cref="TermsEnum"/> from the given <seealso cref="Terms"/>. this method allows certain parsers
//    /// to filter the actual TermsEnum before the field cache is filled.
//    /// </summary>
//    /// <param name="terms"> the <seealso cref="Terms"/> instance to create the <seealso cref="TermsEnum"/> from. </param>
//    /// <returns> a possibly filtered <seealso cref="TermsEnum"/> instance, this method must not return <code>null</code>. </returns>
//    /// <exception cref="IOException"> if an <seealso cref="IOException"/> occurs </exception>
//    TermsEnum TermsEnum(Terms terms);
//  }

//  [Obsolete]
//  public interface FieldCache_ByteParser : FieldCache_Parser
//  {
//    /// <summary>
//    /// Return a single Byte representation of this field's value. </summary>
//    sbyte ParseByte(BytesRef term);
//  }

//  [Obsolete]
//  public interface FieldCache_ShortParser : FieldCache_Parser
//  {
//    /// <summary>
//    /// Return a short representation of this field's value. </summary>
//    short ParseShort(BytesRef term);
//  }

//  public interface FieldCache_IntParser : FieldCache_Parser
//  {
//    /// <summary>
//    /// Return an integer representation of this field's value. </summary>
//    int ParseInt(BytesRef term);
//  }

//  public interface FieldCache_FloatParser : FieldCache_Parser
//  {
//    /// <summary>
//    /// Return an float representation of this field's value. </summary>
//    float ParseFloat(BytesRef term);
//  }

//  public interface FieldCache_LongParser : FieldCache_Parser
//  {
//    /// <summary>
//    /// Return an long representation of this field's value. </summary>
//    long ParseLong(BytesRef term);
//  }

//  public interface FieldCache_DoubleParser : FieldCache_Parser
//  {
//    /// <summary>
//    /// Return an double representation of this field's value. </summary>
//    double ParseDouble(BytesRef term);
//  }

//  public sealed class FieldCache_CacheEntry
//  {
//    private readonly object readerKey;
//    private readonly string fieldName;
//    private readonly Type cacheType;
//    private readonly object custom;
//    private readonly object value;
//    private string Size;

//    public FieldCache_CacheEntry(object readerKey, string fieldName, Type cacheType, object custom, object value)
//    {
//      this.readerKey = readerKey;
//      this.fieldName = fieldName;
//      this.cacheType = cacheType;
//      this.custom = custom;
//      this.value = value;
//    }

//    public object ReaderKey
//    {
//        get
//        {
//          return readerKey;
//        }
//    }

//    public string FieldName
//    {
//        get
//        {
//          return fieldName;
//        }
//    }

//    public Type CacheType
//    {
//        get
//        {
//          return cacheType;
//        }
//    }

//    public object Custom
//    {
//        get
//        {
//          return custom;
//        }
//    }

//    public object Value
//    {
//        get
//        {
//          return value;
//        }
//    }

//    /// <summary>
//    /// Computes (and stores) the estimated size of the cache Value </summary>
//    /// <seealso cref= #getEstimatedSize </seealso>
//    public void EstimateSize()
//    {
//      long bytesUsed = RamUsageEstimator.SizeOf(Value);
//      Size = RamUsageEstimator.HumanReadableUnits(bytesUsed);
//    }

//    /// <summary>
//    /// The most recently estimated size of the value, null unless
//    /// estimateSize has been called.
//    /// </summary>
//    public string EstimatedSize
//    {
//        get
//        {
//          return Size;
//        }
//    }

//    public override string ToString()
//    {
//      StringBuilder b = new StringBuilder();
//      b.Append("'").Append(ReaderKey).Append("'=>");
//      b.Append("'").Append(FieldName).Append("',");
//      b.Append(CacheType).Append(",").Append(Custom);
//      b.Append("=>").Append(Value.GetType().Name).Append("#");
//      b.Append(System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(Value));

//      string s = EstimatedSize;
//      if (null != s)
//      {
//        b.Append(" (size =~ ").Append(s).Append(')');
//      }

//      return b.ToString();
//    }
}