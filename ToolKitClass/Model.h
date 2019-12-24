#pragma once
#include <iostream>
#include <vector>
#include <assimp/Importer.hpp>
#include <assimp/scene.h>
#include <assimp/postprocess.h>
#include "Shader.h"
#include "Mesh.h"
class Model
{
public:
	Model(char *path);
	void Draw(Shader shader);
	~Model();
private:
	/*ģ������*/
	std::vector<Mesh> meshes;
	std::string directory;
	std::vector<Texture> textures_loaded;

	/*����*/
	void loadModel(std::string path);
	void processNode(aiNode *node, const aiScene *scene);
	Mesh processMesh(aiMesh *mesh, const aiScene *scene);
	std::vector<Texture> loadMaterialTextures(aiMaterial *mat, aiTextureType type, std::string typeName);
	unsigned int TextureFromFile(const char *path, const std::string &directory, bool gamma= false);
};

