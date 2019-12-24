#pragma once
//顶点结构体
#include <glad/glad.h>
#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <string>
#include <vector>
#include <assimp/scene.h>
#include "Shader.h"

struct Vertex {
	glm::vec3 Position;
	glm::vec3 Normal;
	glm::vec2 TexCoords;
	glm::vec3 Tangent;//切线
	glm::vec3 Bitangent;
};

struct Texture {
	unsigned int id;
	std::string type;//纹理类型
	aiString path;//纹理路径
};


class Mesh
{
public:
	Mesh(std::vector<Vertex> vertices,std::vector<unsigned int> indices,std::vector<Texture> textures);
	~Mesh();

	void Draw(Shader shader);
public:
	/*网格数据*/
	std::vector<Vertex> vertices;
	std::vector<unsigned int> indices;
	std::vector<Texture> textures;
	unsigned int VAO;
private:
	void setupMesh();
private:
	unsigned int VBO, EBO;
};

