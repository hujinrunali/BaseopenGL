#version 330 core
out vec4 FragColor;

in vec3 Normal;  
in vec3 FragPos;  
in vec3 LightPos;
  
uniform vec3 lightColor;
uniform vec3 objectColor;

void main()
{
    //环境光的强度
    float ambientStrength = 0.1;
    vec3 ambient = ambientStrength * lightColor;
  	
    // 漫反射光的强度 
    vec3 norm = normalize(Normal);
    vec3 lightDir = normalize(LightPos- FragPos);//由物体指向光源
    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = diff * lightColor;
     
    //镜面反射光的强度
    float specularStrength = 1.0;
    vec3 viewDir = normalize(-FragPos);
    vec3 reflectDir = reflect(-lightDir,norm);//reflect要求第一个向量为从光源指向切片
    float spec = pow(max(dot(viewDir,reflectDir),0.0),32);
    vec3 specular = specularStrength * spec*lightColor;

    vec3 result = (ambient + diffuse + specular) * objectColor;
    FragColor = vec4(result, 1.0);
} 