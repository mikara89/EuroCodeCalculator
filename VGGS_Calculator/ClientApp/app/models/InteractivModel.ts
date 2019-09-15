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
    'εs1': number;
    'εs2': number;
    'εc1': number;
    'εc2': number;
    'x': number;
}
interface InteractivModelDetails {
    'extrims': InteractivModelItem[],
    'isValid': boolean,
    'worrnings': string[]
}

       