struct Vertex    //Overloaded Vertex Structure
{
    Vertex() {}
    Vertex(float x, float y, float z)
        : pos(x, y, z) {}

    XMFLOAT3 pos;
};

D3D11_INPUT_ELEMENT_DESC layout[] =
{
    { "POSITION", 0, DXGI_FORMAT_R32G32B32_FLOAT, 0, 0, D3D11_INPUT_PER_VERTEX_DATA, 0 },
};