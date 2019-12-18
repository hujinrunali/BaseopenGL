#pragma once
#include <glad/glad.h>
#include <glm/glm.hpp>
#include <string>
#include <fstream>
#include <sstream>
#include <iostream>


class Shader
{
public:
	Shader(const GLchar* vertexPath,const GLchar* fragmentPath);
	~Shader();
public:
	//使用/激活函数
	void use();
	//uniform工具函数
	void setBool(const std::string &name, bool value) const;
	void setInt(const std::string &name, int value) const;
	void setFloat(const std::string&name, float value) const;
	void setMat4(const std::string&name, const glm::mat4 &value) const;
	void setVec3(const std::string&name, const float R, const float G, const float B) const;
	void setVec3(const std::string&name, const glm::vec3 &value) const;
public:
	unsigned int ID;
};

