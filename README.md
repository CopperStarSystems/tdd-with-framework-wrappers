### Stuck?  Want More Information?
#### Contact Me on CodeMentor for Live 1:1 Guidance!
[![Get help on Codementor](https://cdn.codementor.io/badges/get_help_github.svg)](https://www.codementor.io/copperstarconsulting?utm_source=github&utm_medium=button&utm_term=copperstarconsulting&utm_campaign=github)


# Improve Testability by Wrapping Static Classes
Certain parts of the .NET framework are exposed as static classes, for example `File`, `Directory`, and `Path`.  Because static classes cannot be mocked, it is very difficult to test classes that use static classes.  Furthermore, some classes may have external side effects or dependencies, for example `File.ReadAllText` requires an actual physical file to reside at the specified location or the call will fail.  This complicates our testing code by requiring us to create a file in a known location for the test, and then clean up afterwards.  This also violates the "Test in Isolation" rule because the test now has a dependency on an actual underlying filesystem.

This post will demonstrate a clean, relatively low-effort way to improve testability for classes that consume static classes.

# GitHub Repository
The project is hosted in a [GitHub Repository](https://github.com/CopperStarSystems/tdd-with-framework-wrappers) containing two branches:

* [difficult-to-test](https://github.com/CopperStarSystems/tdd-with-framework-wrappers/tree/difficult-to-test):  This branch directly uses `System.Io.File` to read a file and illustrates a couple of common challenges when testing code that consumes static classes
* [with-framework-wrappers](https://github.com/CopperStarSystems/tdd-with-framework-wrappers/tree/with-framework-wrappers):  This branch introduces a simple wrapper to encapsulate calls to `System.Io.File` and improve testability

# Solution Overview
The solution is a simple example, consisting of two projects:
* Tdd.FrameworkWrappers.Lib:  The implementation under test.  It contains a single class, `FileReader`, which reads a file from storage and returns its contents as a string.
* Tdd.FrameworkWrappers.Lib.Tests:  NUnit tests for the `Tdd.FrameworkWrappers.Lib` project.

# Walkthrough:  `difficult-to-test` branch
We'll start by cloning the `difficult-to-test` branch and opening the solution.

If we take a look at `FileReader.cs`, we can see that its `ReadText` method simply delegates to `System.Io.File.ReadAllText` and returns its result.

Things get more convoluted, however, when we dig into `FileReaderTests.cs`.  The direct usage of `System.Io.File` complicates our testing in a few ways:
* We cannot verify the interaction between `FileReader` and `System.Io.File` because static classes cannot be mocked
* We need to add pre-and-post test methods to create and clean up a test file on the system.  This in itself can be problematic since reading/writing/deleting files can be error-prone (i.e. IoExceptions), so our test could conceivably fail for reasons outside the bounds of the test.

# Wrappers to the Rescue!
We can solve this testability issue by wrapping `System.Io.File` in another, non-static class that we control.  By doing so, we can expose an `IFile` interface (so we can mock and support dependency injection).  The `IFile` interface is implemented by `FileImpl`, which would be the concrete type injected into instances of `FileReader` by our IoC container.

So without further ado, let's dig in!

# Walkthrough:  `with-framework-wrappers` branch
Now let's clone the `with-framework-wrappers` branch and open the solution.

We'll start in the same place as before, in `FileReader.cs`.  Note that there is now a constructor that takes an `IFile` and `ILogger` instance, and that the `ReadText` method now invokes `IFile.ReadAllText` instead of `File.ReadAllText`.

You'll also notice that there is a new `FrameworkWrappers` folder, so let's dig into that.  `FrameworkWrappers` contains `IFile` and its implementation, `FileImpl`.  Looking at the interface, we see that it exposes a `ReadAllText` method whose signature matches that of `System.Io.File.ReadAllText`.  Moving on to `FileImpl`, we see that it's doing exactly what `FileReader` was doing before:  Simply delegating the call to `System.Io.File.ReadAllText` and returning the result.

If we now move on to the tests, we'll see some more changes, specifically:
- The `CreateTestFile` and `CleanupTestFile` methods are gone because we no longer have to write to a filesystem in order to execute this test (e.g. we have removed the test's dependency on the underlying filesystem)
- `SetUp` has been modified to instantiate a `Mock<IFile>` and `Mock<ILogger>` and inject them into the `FileReader` under test
- `ReadText_WhenFileExists_ReturnsFileContents` is substantially simplified
- `ReadText_Always_PerformsExpectedWork` can now be implemented - because we're using a mock, we can verify that the `ReadText` method does, in fact, call the `ReadAllText` method on `IFile` with the expected arguments.
- We can also now test 'sad' flows, e.g. "Does `FileReader` react appropriately if an exception is thrown by `IFileReader.ReadAllText`?"  `ReadText_WhenIoExceptionThrown_PerformsExpectedWork` illustrates this new capability
- Test cases are parameterized using `TestCaseSourceAttribute`.  This isn't really specific to testing with Framework Wrappers, but it's just a way to reduce duplication.

You may notice that there is no corresponding `FileImplTests` in the test project, this is normal for __pure__ wrappers, e.g. classes that simply delegate calls to an external class (e.g. a Microsoft library) without adding any logic.  The reasoning behind this is the expectation that Microsoft (or the third-party vendor) would be responsible for testing their own libraries, so testing the wrapper would be redundant.

# Wind-Down
So in a nutshell, what we did here was improve testability by adding a thin layer of indirection around the `System.Io.File` class, enabling us to introduce an interface (for DI and Mocking), implemented by a class we control.  The end result is that we can effectively test our `FileReader` class in complete isolation from external dependencies.

I tend to follow this pattern almost any time I need to consume a static class, for example `File`, `Directory`, `Path`, `ConfigurationManager`, etc.  I've also used a variation of this pattern with WPF applications, wrapping non-WPF-friendly UI components in an Attached Behavior to facilitate communications between ViewModels and legacy UI components.

### Stuck?  Want More Information?
#### Contact Me on CodeMentor for Live 1:1 Guidance!
[![Get help on Codementor](https://cdn.codementor.io/badges/get_help_github.svg)](https://www.codementor.io/copperstarconsulting?utm_source=github&utm_medium=button&utm_term=copperstarconsulting&utm_campaign=github)
