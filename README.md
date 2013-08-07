TreeTraversalPerformance
========================

Simple console test app measuring performance of visiting all nodes in a simple tree via Enumerator or Traverse with callback.
It demonstrates a difference in performance between Enumerator and the Visitor method that surprised me.

Example run
===========

t0 Node Count 5420
t0 largest child count 61
 DoTraverseListTest totalTime 1
 DoTraverseListTest perRun 0.0025
 DoTraverseListTest perNode 4.61254612546125E-07
 DoEnumeratorListTest totalTime 81
 DoEnumeratorListTest perRun 0.81
 DoTraverseListTest perNode 0.000149446494464945
 Enumerator / Traverse 324

t1 Node Count 44019
t1 largest child count 80
 DoTraverseListTest totalTime 7
 DoTraverseListTest perRun 0.175
 DoTraverseListTest perNode 3.97555600990481E-06
 DoEnumeratorListTest totalTime 91
 DoEnumeratorListTest perRun 9.1
 DoTraverseListTest perNode 0.00020672891251505
 Enumerator / Traverse 52

t2 Node Count 602209
t2 largest child count 107
 DoTraverseListTest totalTime 105
 DoTraverseListTest perRun 2.625
 DoTraverseListTest perNode 4.358951792484E-06
 DoEnumeratorListTest totalTime 1454
 DoEnumeratorListTest perRun 145.4
 DoTraverseListTest perNode 0.000241444415477019
 Enumerator / Traverse 55.3904761904762

t3 Node Count 7036608
t3 largest child count 317
 DoTraverseListTest totalTime 1306
 DoTraverseListTest perRun 32.65
 DoTraverseListTest perNode 4.64001973678227E-06
 DoEnumeratorListTest totalTime 19267
 DoEnumeratorListTest perRun 1926.7
 DoTraverseListTest perNode 0.000273810904344821
 Enumerator / Traverse 59.010719754977

