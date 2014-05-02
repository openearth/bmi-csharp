// SampleCppLibrary.cpp : Defines the exported functions for the DLL application.
//

#include <stdio.h>
#include "bmi.h"
#include <string.h>


// TODO: move to a separate BMI C/C++ project.

BMI_API void initialize(const char* config_file)
{
	printf("initializing ...\n");
}

BMI_API void update(const double& dt)
{
	printf("updating ...\n");
}

BMI_API void finalize()
{
	printf("finalizing ...\n");
}

BMI_API void get_start_time(double& t)
{
}

BMI_API void get_end_time(const double& t)
{
}

BMI_API void get_current_time(double& t)
{
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

