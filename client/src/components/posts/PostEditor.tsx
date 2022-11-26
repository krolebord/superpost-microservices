import { useMutation, useQueryClient } from "@tanstack/react-query";
import clsx from "clsx";
import { FC, useState } from "react";
import { useNavigate } from "react-router-dom";
import { createPost } from "../../api/posts";
import { postTimelineQuery } from "../../api/timeline";

type PostEditorProps = {
  parentPostId?: string
  className?: string;
  maxContentLength?: number;
}

export const PostEditor: FC<PostEditorProps> = (props) => {
  const { parentPostId, className, maxContentLength = 250 } = props;

  const navigate = useNavigate();
  const client = useQueryClient();
  const postMutation = useMutation({
    mutationFn: createPost,
    onSuccess: (postId) => {
      if (parentPostId) {
        client.invalidateQueries(postTimelineQuery(parentPostId).queryKey);
      }
      else {
        navigate(`/post/${postId}`);
      }
    }
  });

  const onPost = () => {
    if (content.length <= 0 || content.length > maxContentLength) {
      return; 
    }
    
    postMutation.mutate({
      content: content,
      parentPostId: parentPostId,
    });
    setContent('');
  };

  const [content, setContent] = useState('');

  return (<div className={clsx('block p-3 bg-slate-700/50 rounded-xl', className)}>
    <textarea
      className="w-full bg-slate-700 rounded-xl py-1 px-2 resize-none"
      placeholder="Write subpost..."
      rows={Math.max(2, content.split('\n').length)}
      value={content}
      onChange={e => setContent(e.target.value)}
    />
    <div className="flex justify-between items-start">
      <span>{content.length} / {maxContentLength}</span>
      <button onClick={onPost} className="bg-purple-500/70 px-6 py-1 rounded-xl hover:bg-purple-500/80">Post</button>
    </div>
  </div>);
}
