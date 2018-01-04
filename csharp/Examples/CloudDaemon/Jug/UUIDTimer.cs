/* JUG Java Uuid Generator
 *
 * Copyright (c) 2002,2010 Tatu Saloranta, tatu.saloranta@iki.fi, Tommi S.E. Laukkanen, tommi.s.e.laukkanen@gmail.com
 *
 * Licensed under the License specified in the file LICENSE which is
 * included with the source code.
 * You may not use this file except in compliance with the License.
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Threading;

namespace Jug
{

    /**
     * UUIDTimer produces the time stamps required for time-based UUIDs.
     * It works as outlined in the UUID specification, with following
     * implementation:
     *<ul>
     *<li>Java classes can only product time stamps with maximum resolution
     *   of one millisecond (at least before JDK 1.5).
     *   To compensate, an additional counter is used,
     *   so that more than one UUID can be generated between java clock
     *   updates. Counter may be used to generate up to 10000 UUIDs for
     *   each distrinct java clock value.
     *<li>Due to even lower clock resolution on some platforms (older
     *  Windows versions use 55 msec resolution), timestamp value can
     *  also advanced ahead of physical value within limits (by default,
     *  up 100 millisecond ahead of reported), iff necessary (ie. 10000
     *  instances created before clock time advances).
     *<li>As an additional precaution, counter is initialized not to 0
     *   but to a random 8-bit number, and each time clock changes, lowest
     *   8-bits of counter are preserved. The purpose it to make likelyhood
     *   of multi-JVM multi-instance generators to collide, without significantly
     *   reducing max. UUID generation speed. Note though that using more than
     *   one generator (from separate JVMs) is strongly discouraged, so
     *   hopefully this enhancement isn't needed.
     *   This 8-bit offset has to be reduced from total max. UUID count to
     *   preserve ordering property of UUIDs (ie. one can see which UUID
     *   was generated first for given UUID generator); the resulting
     *   9500 UUIDs isn't much different from the optimal choice.
     *<li>Finally, as of version 2.0 and onwards, optional external timestamp
     *   synchronization can be done. This is done similar to the way UUID
     *   specification suggests; except that since there is no way to 
     *   lock the whole system, file-based locking is used. This works
     *   between multiple JVMs and Jug instances.
     *</ul>
     *<p>
     *Some additional assumptions about calculating the timestamp:
     *<ul>
     *<li>System.currentTimeMillis() is assumed to give time offset in UTC,
     *   or at least close enough thing to get correct timestamps. The
     *   alternate route would have to go through calendar object, use
     *   TimeZone offset to get to UTC, and then modify. Using currentTimeMillis
     *   should be much faster to allow rapid UUID creation.
     *<li>Similarly, the constant used for time offset between 1.1.1970 and
     *  start of Gregorian calendar is assumed to be correct (which seems
     *  to be the case when testing with Java calendars).
     *</ul>
     *<p>
     * Note about synchronization: this class is assumed to always be called
     * from a synchronized context (caller locks on either this object, or
     * a similar timer lock), and so has no method synchronization.
     */
    public class UUIDTimer
    {
        // // // Constants

        /**
         * Since System.longTimeMillis() returns time from january 1st 1970,
         * and UUIDs need time from the beginning of gregorian calendar
         * (15-oct-1582), need to apply the offset:
         */
        private const long kClockOffset = 0x01b21dd213814000L;
        /**
         * Also, instead of getting time in units of 100nsecs, we get something
         * with max resolution of 1 msec... and need the multiplier as well
         */
        private const long kClockMultiplier = 10000;
        private const long kClockMultiplierL = 10000L;

        /**
         * Let's allow "virtual" system time to advance at most 100 milliseconds
         * beyond actual physical system time, before adding delays.
         */
        private const long kMaxClockAdvance = 100L;

        // // // Configuration

        private Random mRnd;

        // // // Clock state:

        /**
         * Additional state information used to protect against anomalous
         * cases (clock time going backwards, node id getting mixed up).
         * Third byte is actually used for seeding counter on counter
         * overflow.
         */
        private byte[] mClockSequence = new byte[3];

        /**
         * Last physical timestamp value <code>System.currentTimeMillis()</code>
         * returned: used to catch (and report) cases where system clock
         * goes backwards. Is also used to limit "drifting", that is, amount
         * timestamps used can differ from the system time value. This value
         * is not guaranteed to be monotonically increasing.
         */
        private long mLastSystemTimestamp = 0L;

        /**
         * Timestamp value last used for generating a UUID (along with
         * {@link #mClockCounter}. Usually the same as
         * {@link #mLastSystemTimestamp}, but not always (system clock
         * moved backwards). Note that this value is guaranteed to be
         * monotonically increasing; that is, at given absolute time points
         * t1 and t2 (where t2 is after t1), t1 <= t2 will always hold true.
         */
        private long mLastUsedTimestamp = 0L;

        /**
         * First timestamp that can NOT be used without synchronizing
         * using synchronization object ({@link #mSync}). Only used when
         * external timestamp synchronization (and persistence) is used,
         * ie. when {@link #mSync} is not null.
         */
        private long mFirstUnsafeTimestamp = long.MaxValue;

        /**
         * Counter used to compensate inadequate resolution of JDK system
         * timer.
         */
        private int mClockCounter = 0;

        /**
         * Object used to reliably ensure that no multiple JVMs
         * generate UUIDs, and also that the time stamp value used for
         * generating time-based UUIDs is monotonically increasing
         * even if system clock moves backwards over a reboot (usually
         * due to some system level problem).
         *<p>
         * See {@link TimestampSynchronizer} for details.
         */
        private TimestampSynchronizer mSync = null;

        public UUIDTimer(Random rnd)
        {
            mRnd = rnd;
            InitCounters(rnd);
            mLastSystemTimestamp = 0L;
            // This may get overwritten by the synchronizer
            mLastUsedTimestamp = 0L;
        }

        private void InitCounters(Random rnd)
        {
            /* Let's generate the clock sequence field now; as with counter,
             * this reduces likelihood of collisions (as explained in UUID specs)
             */
            rnd.NextBytes(mClockSequence);
            /* Ok, let's also initialize the counter...
             * Counter is used to make it slightly less likely that
             * two instances of UUIDGenerator (from separate JVMs as no more
             * than one can be created in one JVM) would produce colliding
             * time-based UUIDs. The practice of using multiple generators,
             * is strongly discouraged, of course, but just in case...
             */
            mClockCounter = mClockSequence[2] & 0xFF;
        }

        public void GetTimestamp(byte[] uuidData)
        {
            // First the clock sequence:
            uuidData[UUID.INDEX_CLOCK_SEQUENCE] = mClockSequence[0];
            uuidData[UUID.INDEX_CLOCK_SEQUENCE + 1] = mClockSequence[1];

            long systime = System.Environment.TickCount;

            /* Let's first verify that the system time is not going backwards;
             * independent of whether we can use it:
             */
            if (systime < mLastSystemTimestamp)
            {
                throw new Exception("System time going backwards! (got value " + systime + ", last " + mLastSystemTimestamp);
            }

            /* But even without it going backwards, it may be less than the
             * last one used (when generating UUIDs fast with coarse clock
             * resolution; or if clock has gone backwards over reboot etc).
             */
            if (systime <= mLastUsedTimestamp)
            {
                /* Can we just use the last time stamp (ok if the counter
                 * hasn't hit max yet)
                 */
                if (mClockCounter < kClockMultiplier)
                { // yup, still have room
                    systime = mLastUsedTimestamp;
                }
                else
                { // nope, have to roll over to next value and maybe wait
                    long actDiff = mLastUsedTimestamp - systime;
                    long origTime = systime;
                    systime = mLastUsedTimestamp + 1L;

                    /* Clock counter is now at exactly the multiplier; no use
                     * just anding its value. So, we better get some random
                     * numbers instead...
                     */
                    InitCounters(mRnd);

                    /* But do we also need to slow down? (to try to keep virtual
                     * time close to physical time; ie. either catch up when
                     * system clock has been moved backwards, or when coarse
                     * clock resolution has forced us to advance virtual timer
                     * too far)
                     */
                    if (actDiff >= kMaxClockAdvance)
                    {
                        SlowDown(origTime, actDiff);
                    }
                }
            }
            else
            {
                /* Clock has advanced normally; just need to make sure counter is
                 * reset to a low value (need not be 0; good to leave a small
                 * residual to further decrease collisions)
                 */
                mClockCounter &= 0xFF;
            }

            mLastUsedTimestamp = systime;

            /* Ok, we have consistent clock (virtual or physical) value that
             * we can and should use.
             * But do we need to check external syncing now?
             */
            if (mSync != null && systime >= mFirstUnsafeTimestamp)
            {
                mFirstUnsafeTimestamp = mSync.Update(systime);
            }

            /* Now, let's translate the timestamp to one UUID needs, 100ns
             * unit offset from the beginning of Gregorian calendar...
             */
            systime *= kClockMultiplierL;
            systime += kClockOffset;

            // Plus add the clock counter:
            systime += mClockCounter;
            // and then increase
            ++mClockCounter;

            /* Time fields are nicely split across the UUID, so can't just
             * linearly dump the stamp:
             */
            int clockHi = (int)(((ulong)systime) >> 32);
            int clockLo = (int)systime;

            uuidData[UUID.INDEX_CLOCK_HI] = (byte)(clockHi >> 24);
            uuidData[UUID.INDEX_CLOCK_HI + 1] = (byte)(clockHi >> 16);
            uuidData[UUID.INDEX_CLOCK_MID] = (byte)(clockHi >> 8);
            uuidData[UUID.INDEX_CLOCK_MID + 1] = (byte)clockHi;

            uuidData[UUID.INDEX_CLOCK_LO] = (byte)(clockLo >> 24);
            uuidData[UUID.INDEX_CLOCK_LO + 1] = (byte)(clockLo >> 16);
            uuidData[UUID.INDEX_CLOCK_LO + 2] = (byte)(clockLo >> 8);
            uuidData[UUID.INDEX_CLOCK_LO + 3] = (byte)clockLo;
        }

        public void SetSynchronizer(TimestampSynchronizer sync)
        {
            TimestampSynchronizer old = mSync;

            if (old != null)
            {
                old.Deactivate();
            }

            mSync = sync;

            /* Ok, now; synchronizer can tell us what is the first timestamp
             * value that definitely was NOT used by the previous incarnation.
             * This can serve as the last used time stamp, assuming it is not
             * less than value we are using now.
             */
            if (sync != null)
            {
                long lastSaved = sync.Initialize();
                if (lastSaved > mLastUsedTimestamp)
                {
                    mLastUsedTimestamp = lastSaved;
                }
            }

            /* Also, we need to make sure there are now no safe values (since
             * synchronizer is not yet requested to allocate any):
             */
            mFirstUnsafeTimestamp = 0L; // ie. will always trigger sync.update()
        }

        /*
        ///////////////////////////////////////////////////////////
        // Private methods
        ///////////////////////////////////////////////////////////
         */

        private const int MAX_WAIT_COUNT = 50;

        /**
         * Simple utility method to use to wait for couple of milliseconds,
         * to let system clock hopefully advance closer to the virtual
         * timestamps used. Delay is kept to just a millisecond or two,
         * to prevent excessive blocking; but that should be enough to
         * eventually synchronize physical clock with virtual clock values
         * used for UUIDs.
         *
         * @param msecs Number of milliseconds to wait for from current 
         *    time point
         */
        private static void SlowDown(long startTime, long actDiff)
        {
            /* First, let's determine how long we'd like to wait.
             * This is based on how far ahead are we as of now.
             */
            long ratio = actDiff / kMaxClockAdvance;
            long delay;

            if (ratio < 2L)
            { // 200 msecs or less
                delay = 1L;
            }
            else if (ratio < 10L)
            { // 1 second or less
                delay = 2L;
            }
            else if (ratio < 600L)
            { // 1 minute or less
                delay = 3L;
            }
            else
            {
                delay = 5L;
            }
            long waitUntil = startTime + delay;
            int counter = 0;
            do
            {
                try
                {
                    Thread.Sleep((int)delay);
                }
                catch (ThreadInterruptedException) { }
                delay = 1L;
                /* This is just a sanity check: don't want an "infinite"
                 * loop if clock happened to be moved backwards by, say,
                 * an hour...
                 */
                if (++counter > MAX_WAIT_COUNT)
                {
                    break;
                }
            } while (System.Environment.TickCount < waitUntil);
        }
    }

}