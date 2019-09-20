interface InteractivModel {
    'M': number;
    'N': number;
}
interface InteractivModelItem {
    'm_Rd': number,
    'n_Rd': number,
    'fs1': number;
    'fs2': number;
    'fc': number;
    'sig_c': number;
    'sig_s1': number;
    'sig_s2': number;
    'eps_s1': number;
    'eps_s2': number;
    'eps_c1': number;
    'eps_c2': number;
    'x': number;
}
interface InteractivModelDetails {
    'extrims': InteractivModelItem[],
    'isValid': boolean,
    'worrnings': string[]
}

interface CalcModel {
    mi: number,
    ni: number,
    geometry: Geometry,
    material: Material
}

interface Geometry {
    b: number;
    h: number;
    d1: number;
    d2: number;
    as1: number;
    as2: number;
}
interface Material {
    armtype: string;
    betonClass: string;
}



       