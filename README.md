[![Build Status](https://tuitionassistant.visualstudio.com/_apis/public/build/definitions/df358588-f09c-47b3-bd42-80c57eb563c1/1/badge)](https://tuitionassistant.visualstudio.com/MyFirstProject/_build/index?definitionId=1)


# Spring2018CourseProjectNelnet
Spring2018CourseProjectNelnet

## Running the web project
Make sure you have Node installed on your computer. You can test this by typing `npm` in a terminal (I recommend either Git Bash or Powershell for Windows), and seeing the following output:
```
Usage: npm <command>

where <command> is one of:
...
```

If this fails, download NodeJs from [here](https://nodejs.org/en/download/) and install.
> If you still cannot use the `npm` command, it likely did not get added to your [path](https://stackoverflow.com/questions/27864040/fixing-npm-path-in-windows-8-and-10)

Navigate to the project using the terminal of your choice, and then go to `NelnetProject > Web`.

Install the extra packages to run the project from that directory with the command `npm install`.

Begin the process that will compile your front-end code by running `npm run watch`. Keep this terminal running, as it will scan the files in the `Nelnet_UI` folder for changes and then recompile when you save. This will save a ton of development time, because you can keep Visual Studio running, and just refresh the page when making UI changes.

Now, open the solution in Visual Studio. Right-click the Web project, set it as the start up project, and run.
