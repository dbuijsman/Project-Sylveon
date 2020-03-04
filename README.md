Project Sylveon

The goal of this project is to develop an application that will test if a candidate has some skills in programming and how fast they can learn new things.
The test is a small game where the candidate should try to get the caracter to the finish using code. The code can be build be using pre-written commands. At the moment there are no limitations on using each block, but the score depends on the solutions that are given by the candidate. After the candidate is finished, the interviewer can see the results on the admin page. These results are the result of analysing the attempts that the candidate did during the test.

This project is not yet ready for production. There are some features that are not fully implemented yet, such as:
- There is no verification yet for logging in on the admin page. However, after log in the client will get a token which is used for authentication.
- The tutorial of the the test is not finished. There is still some information missing on this page.
- There are only 3 easy levels yet. These levels are currently used for testing the application. There will be more and harded levels.
- There is no point yet of introducing new information to the candidate during the test. This will be implemented and the results will be analysed.
- The amount of different commands is quite limited. There are some commands that are ready to be implemented in the front-end but there will be more (existing and custom) commands in the future.
- The results shows only some limited information. At the moment it will only analyse the shortest solution of the candidate. This information include the amount of lines, the duration in order to get to this solution and the number of attempts to find this solution. This will be extended and improved. For example, there will be results about the number of attempts that has an infinite loop, the usage of flow control and how much time and effort the candidate spends in finding a better solution.
- There is some logic that should be moved to different files. This is currently in progress.
