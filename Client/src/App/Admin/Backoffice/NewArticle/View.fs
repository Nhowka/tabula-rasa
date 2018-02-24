module Admin.Backoffice.NewArticle.View

open Admin.Backoffice.NewArticle.Types

open Fable.Helpers.React.Props
open Fable.Helpers.React
open Fable.Core.JsInterop
open React.Marked

let title state dispatch = 
  let publishBtnContent = 
    if state.IsPublishing 
    then i [ ClassName "fa fa-circle-o-notch fa-spin" ] [ ]
    else str "Publish" 
    
  h1 
    [ ] 
    [ str "New Post" 
      button 
        [ ClassName "btn btn-info"
          Style [ MarginLeft 15 ]
          OnClick (fun _ -> dispatch TogglePreview) ] 
        [ str (if state.Preview then "Back to Post" else "Preview") ]
      button 
        [ ClassName "btn btn-success"
          Style [ MarginLeft 15 ]
          OnClick (fun _ -> dispatch Publish) ] 
        [ publishBtnContent ] ]
        
let onTextChanged disptach = 
  OnChange <| fun (ev: Fable.Import.React.FormEvent) ->
    let value : string = !!ev.target?value
    value |> disptach 

let spacing = Style [ Margin 5 ]
 
let contentEditor state dispatch = 
   div 
     [ ClassName "form-group"; spacing ]
     [ label [] [ str "Content" ]
       textarea 
           [ ClassName "form-control"
             Rows 13.0 
             DefaultValue state.Content
             onTextChanged (SetContent >> dispatch) ] 
           [ ] ]

let tagsEditor state dispatch = 
  let btnStyle = 
    if List.contains state.NewTag state.Tags || state.NewTag = ""
    then "btn btn-info"
    else "btn btn-success"
    
  form  
    [ ClassName "form-inline"; Style [ MarginLeft 10 ] ]
    [ div
        [ ClassName "form-group" ]
        [ str "Tags" ] 
      div 
        [ ClassName "form-group mx-sm-3" ] 
        [ label [ ClassName "sr-only" ] [ str "Tags: " ] 
          input [ ClassName "form-control";
                  DefaultValue state.NewTag
                  Placeholder "New Tag"
                  onTextChanged (SetTag >> dispatch) ] ]  
      div 
        [ ClassName ("btn " + btnStyle)
          OnClick (fun _ -> dispatch AddTag) ] 
        [ i [ ClassName "fa fa-plus" ] [ ] ] ]  
        

let titleAndSlug state dispatch = 
  div 
    [ ClassName "row" ]
    [ div 
        [ ClassName "col"; spacing ]
        [ label [ spacing ] [ str "Title" ] 
          input [ ClassName "form-control"; 
                  DefaultValue state.Title
                  onTextChanged (SetTitle >> dispatch)
                  spacing ] ]
      div 
        [ ClassName "col"; spacing ]
        [ label [ spacing ] [ str "Slug" ] 
          input [ ClassName "form-control";
                  DefaultValue state.NewTag
                  onTextChanged (SetSlug >> dispatch)
                  spacing ] ] ] 

let tagsView tags dispatch = 
  let tags = List.rev tags
  if List.isEmpty tags then ofOption None 
  else 
   div [ ClassName "form-group"; Style [ Margin 10 ] ]
       [ for tag in tags -> 
           div [ ClassName "btn btn-info"; 
                 Style [ Margin 5 ]
                 OnClick (fun _ -> (RemoveTag >> dispatch) tag)] 
               [ str tag ] ] 
                
let editor state dispatch = 
  div  
    [ Style [ Margin 10 ] ]
    [ form 
        [ ] 
        [ titleAndSlug state dispatch
          br [ ]
          tagsEditor state dispatch
          tagsView state.Tags dispatch
          br [ ]
          contentEditor state dispatch ] ]

     
let preview state = 
  div 
    [ ClassName "card"; Style [ Padding 20 ] ] 
    [ marked [ Content state.Content; Options [ Sanitize false ] ] ] 

let body state dispatch = 
  if state.Preview 
  then preview state
  else editor state dispatch
  
let render (state : NewArticleState) dispatch = 
    div 
      [ ClassName "container" ]
      [ title state dispatch
        br [ ]
        body state dispatch ] 