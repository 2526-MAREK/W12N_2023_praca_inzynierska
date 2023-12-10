import * as types from "./actionTypes";


export function setIsInitializeModelsJson(state: any)
 {return { type: types.SET_IS_INITIALIZE_MODELS_JSON, isInitilizedModelsStructureJson: state };}
 


 export function setIsInitializeStructureAllObjectWithBasicInfoJson(state: any)
 {return { type: types.SET_IS_INITIALIZE_STRUCTURE_ALL_OBJECT_WITH_BASIC_INFO_JSON, isInitializeStructureAllObjectWithBasicInfoJson: state };}


 export function setIsInitializeStructureAllObjectToFastModifyJson(state: any)
 {return { type: types.SET_IS_INITIALIZE_STRUCTURE_ALL_OBJECT_TO_FAST_MODIFY_JSON,  isInitializeStructureAllObjectToFastModifyJson: state };}