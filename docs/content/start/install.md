+++
date = "2016-06-21T16:34:43+02:00"
description = "How do I get Bench up and running?"
title = "Installing Bench"
weight = 1
+++

[bootstrap-file]: https://github.com/mastersign/bench/raw/master/res/bench-install.bat

At first create a folder for Bench on your harddrive or a thumbdrive.

Bench is usually installed with the bootstrap script
[`bench-install.bat`][bootstrap-file].
Download it by right-clicking and choosing something like _Save target as_
or _Save link_.
Choose the Bench folder, you created before, as the location for saving
the boostrap script.

The current web browsers are quite protective if it comes to downloaded
executable files.
That is why they flag the downloaded script to inform Windows about
the potentially unsave origin of the file.
Windows does not allow the execution of such flagged files.
Mozilla Firefox additionally adds the filename extension `.txt` to prevent
the user from accidently run a downloaded script file.
Because we really want to run the bootstrap file for Bench,
you have to do two things:

1. Remove the additional filename extension `.txt` if it was added
2. Open the properties dialog of the file in the Windows Explorer
   and approve of the file, to allow execution.

Now simply run the `bench-install.bat` and follow the steps in the
initialization wizzard.
After the wizzard is completed, the required apps for Bench are set up,
and the [Bench Dashboard](/ref/dashboard) pops up.

You can find a more detailed description in the tutorial for
[Setting-Up Bench](/tutorial/setup).
