// SampleCppLibrary.cpp : Defines the exported functions for the DLL application.
//

#include <stdio.h>
#include "bmi.h"
#include <string.h>


double current_time = 0.0;
double time_step = 1.0;
logger_callback logger;

enum LEVEL { ALL, DEBUG, INFO, WARN, ERROR, FATAL, OFF };

// TODO: move to a separate BMI C/C++ project.

void log(char* message)
{
	if (logger == NULL)
	{
		printf(message);
	}
	else
	{
		int level = (int)INFO;
		printf("%d : %s\n", level, message);
		printf("%d\n", logger);
		logger(&level, message);
	}
}

BMI_API void initialize(const char* config_file)
{
	log("initializing ...\n");
}

BMI_API void update(const double& dt)
{
	log("updating ...\n");

	if (dt == -1)
	{
		current_time += time_step;
	}
	else
	{
		current_time += dt;
	}
}

BMI_API void finalize()
{
	log("finalizing ...\n");
}

BMI_API void get_start_time(double& t)
{
	t = 0.0;
}

BMI_API void get_end_time(double& t)
{
	t = 5.0;
}

BMI_API void get_current_time(double& t)
{
	t = current_time;
}

BMI_API void get_time_step(double& dt)
{
}

BMI_API void get_string_attribute(const char& name, char& value)
{
}

BMI_API void get_n_variables(int& count) // non-BMI
{
	count = 1;
} 

BMI_API void get_variable_name(const int index, char* variable) // non-BMI
{
	strcpy(variable, "test");
} 

BMI_API void get_var_shape(const char& variable, int* shape)
{
}

BMI_API void get_var_rank(const char& variable, int& rank)
{
}

BMI_API void get_1d_double(const char& variable, double** values)
{
}

BMI_API void get_1d_int(const char& variable, int** values)
{
}

BMI_API void get_2d_int(const char& variable, int*** values)
{
}

BMI_API void set_1d_double_at_index(const char& variable, const int& index, double& value)
{
}

BMI_API void set_logger(logger_callback logger)
{
	::logger = logger;
}
